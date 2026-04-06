using ERDM.Fraud.Service.Application.Behaviors;
using ERDM.Fraud.Service.Application.Commands.Handlers;
using ERDM.Fraud.Service.Application.Mappings;
using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Infrastructure.EventHandlers;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using ERDM.Fraud.Service.Infrastructure.Repositories.Read;
using ERDM.Fraud.Service.Infrastructure.Repositories.Write;
using ERDM.Fraud.Service.Infrastructure.Services;
using ERDMCore.Infrastructure.MongoDB.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog - Use Serilog only
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("MongoDB.Driver", Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithProperty("Application", "ERDM.Fraud.Service.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File($"logs/erdm-fraud-{DateTime.Now:yyyy-MM-dd}.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

builder.Host.UseSerilog();

// Load MongoDB settings
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
if (mongoDbSettings == null)
{
    throw new InvalidOperationException("MongoDB settings are not configured in appsettings.json");
}

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// Register MongoDB Client with proper settings
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<MongoDbSettings>();
    var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
    clientSettings.MinConnectionPoolSize = settings.MinPoolSize;
    clientSettings.MaxConnectionPoolSize = settings.MaxPoolSize;
    clientSettings.ConnectTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeoutSeconds);
    clientSettings.SocketTimeout = TimeSpan.FromSeconds(settings.SocketTimeoutSeconds);
    clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeoutSeconds);
    clientSettings.RetryWrites = settings.RetryWrites;
    clientSettings.RetryReads = settings.RetryReads;

    if (settings.UseSsl)
    {
        clientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
        };
    }

    return new MongoClient(clientSettings);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<MongoDbSettings>();
    return client.GetDatabase(settings.DatabaseName);
});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ERDM Fraud Detection API",
        Description = "API for identity verification, device fingerprinting, and fraud detection",
        Contact = new OpenApiContact
        {
            Name = "ERDM Support",
            Email = "support@erdm.com"
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    c.CustomSchemaIds(type => type.FullName);
    c.UseAllOfForInheritance();
    c.UseOneOfForPolymorphism();
});

// Add CQRS MediatR - Register all handlers from assembly
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(RegisterDeviceCommandHandler).Assembly);
    // Add pipeline behaviors
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register Write Repositories (Command Side)
builder.Services.AddScoped<IDeviceFingerprintWriteRepository, DeviceFingerprintWriteRepository>();
builder.Services.AddScoped<IIdentityVerificationWriteRepository, IdentityVerificationWriteRepository>();
builder.Services.AddScoped<IFraudCaseWriteRepository, FraudCaseWriteRepository>();

// Register Read Repositories (Query Side)
builder.Services.AddScoped<IDeviceFingerprintReadRepository, DeviceFingerprintReadRepository>();
builder.Services.AddScoped<IFraudCaseReadRepository, FraudCaseReadRepository>();

// Register External Services
builder.Services.AddScoped<IIdentityVerificationService, IdentityVerificationService>();
builder.Services.AddScoped<IBiometricService, BiometricService>();
builder.Services.AddScoped<ISanctionsScreeningService, SanctionsScreeningService>();
builder.Services.AddScoped<ISyntheticIdentityDetectionService, SyntheticIdentityDetectionService>();

builder.Services.AddScoped<IFraudAlertService, FraudAlertService>();
builder.Services.AddScoped<IFraudAlertReadRepository, FraudAlertReadRepository>();
builder.Services.AddScoped<IFraudAlertWriteRepository, FraudAlertWriteRepository>();

// Register Cache Service
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// Register HTTP Client Factory for external API calls
builder.Services.AddHttpClient();

// Register Event Handlers for Projections (Read Side) - MediatR notifications
builder.Services.AddTransient<INotificationHandler<DeviceRegisteredEvent>, DeviceEventHandlers>();
builder.Services.AddTransient<INotificationHandler<DeviceAssociatedEvent>, DeviceEventHandlers>();
builder.Services.AddTransient<INotificationHandler<DeviceRiskScoreUpdatedEvent>, DeviceEventHandlers>();
builder.Services.AddTransient<INotificationHandler<DeviceBlacklistedEvent>, DeviceEventHandlers>();
builder.Services.AddTransient<INotificationHandler<BehavioralPatternDetectedEvent>, DeviceEventHandlers>();
builder.Services.AddTransient<INotificationHandler<IdentityVerificationInitiatedEvent>, IdentityVerificationEventHandlers>();
builder.Services.AddTransient<INotificationHandler<IdentityVerificationCompletedEvent>, IdentityVerificationEventHandlers>();
builder.Services.AddTransient<INotificationHandler<LivenessDetectionCompletedEvent>, IdentityVerificationEventHandlers>();
builder.Services.AddTransient<INotificationHandler<BiometricRegisteredEvent>, IdentityVerificationEventHandlers>();
builder.Services.AddTransient<INotificationHandler<FraudCaseOpenedEvent>, FraudCaseEventHandlers>();
builder.Services.AddTransient<INotificationHandler<FraudEvidenceAddedEvent>, FraudCaseEventHandlers>();
builder.Services.AddTransient<INotificationHandler<FraudCaseLinkedEvent>, FraudCaseEventHandlers>();
builder.Services.AddTransient<INotificationHandler<FraudCaseResolvedEvent>, FraudCaseEventHandlers>();
builder.Services.AddTransient<INotificationHandler<FraudRingDetectedEvent>, FraudCaseEventHandlers>();
builder.Services.AddTransient<INotificationHandler<SanctionsScreeningCompletedEvent>, SanctionsEventHandlers>();
builder.Services.AddTransient<INotificationHandler<SyntheticIdentityDetectedEvent>, SyntheticIdentityEventHandlers>();

// Add AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    // Device Fingerprint Mapping
    cfg.AddProfile<DeviceFingerprintMappingProfile>();

    // Identity Verification Mapping
    cfg.AddProfile<IdentityVerificationMappingProfile>();

    // Fraud Case Mapping
    cfg.AddProfile<FraudCaseMappingProfile>();

    // Fraud Alert Mapping
    cfg.AddProfile<FraudAlertMappingProfile>();
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERDM Fraud API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Test MongoDB connection on startup
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var settings = scope.ServiceProvider.GetRequiredService<MongoDbSettings>();
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

    try
    {
        await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
        logger.LogInformation("Successfully connected to MongoDB Atlas");
        logger.LogInformation("Database: {DatabaseName}", settings.DatabaseName);

        var collections = await (await database.ListCollectionNamesAsync()).ToListAsync();
        logger.LogInformation("Existing collections: {Count}", collections.Count);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to MongoDB");
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
    }
}

// Ensure read database indexes are created
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await CreateReadIndexes(database, logger);
}

app.Run();

async Task CreateReadIndexes(IMongoDatabase database, ILogger<Program> logger)
{
    try
    {
        logger.LogInformation("Creating read database indexes...");

        // Device Fingerprint Read Model Indexes
        var deviceCollection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");

        await deviceCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<DeviceFingerprintReadModel>(
                Builders<DeviceFingerprintReadModel>.IndexKeys.Ascending(x => x.DeviceId),
                new CreateIndexOptions { Unique = true, Name = "idx_device_id" }));

        await deviceCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<DeviceFingerprintReadModel>(
                Builders<DeviceFingerprintReadModel>.IndexKeys.Ascending(x => x.CustomerId),
                new CreateIndexOptions { Name = "idx_customer_id" }));

        await deviceCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<DeviceFingerprintReadModel>(
                Builders<DeviceFingerprintReadModel>.IndexKeys.Ascending(x => x.FingerprintHash),
                new CreateIndexOptions { Unique = true, Name = "idx_fingerprint_hash" }));

        await deviceCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<DeviceFingerprintReadModel>(
                Builders<DeviceFingerprintReadModel>.IndexKeys.Ascending(x => x.RiskScore),
                new CreateIndexOptions { Name = "idx_risk_score" }));

        // Fraud Case Read Model Indexes
        var fraudCaseCollection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");

        await fraudCaseCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<FraudCaseReadModel>(
                Builders<FraudCaseReadModel>.IndexKeys.Ascending(x => x.CaseId),
                new CreateIndexOptions { Unique = true, Name = "idx_case_id" }));

        await fraudCaseCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<FraudCaseReadModel>(
                Builders<FraudCaseReadModel>.IndexKeys.Ascending(x => x.CustomerId),
                new CreateIndexOptions { Name = "idx_customer_id" }));

        await fraudCaseCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<FraudCaseReadModel>(
                Builders<FraudCaseReadModel>.IndexKeys.Ascending(x => x.Status),
                new CreateIndexOptions { Name = "idx_status" }));

        await fraudCaseCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<FraudCaseReadModel>(
                Builders<FraudCaseReadModel>.IndexKeys.Ascending(x => x.RiskScore),
                new CreateIndexOptions { Name = "idx_risk_score" }));

        // Identity Verification Read Model Indexes
        var verificationCollection = database.GetCollection<IdentityVerificationReadModel>("identity_verifications_read");

        await verificationCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<IdentityVerificationReadModel>(
                Builders<IdentityVerificationReadModel>.IndexKeys.Ascending(x => x.VerificationId),
                new CreateIndexOptions { Unique = true, Name = "idx_verification_id" }));

        await verificationCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<IdentityVerificationReadModel>(
                Builders<IdentityVerificationReadModel>.IndexKeys.Ascending(x => x.CustomerId),
                new CreateIndexOptions { Name = "idx_customer_id" }));

        await verificationCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<IdentityVerificationReadModel>(
                Builders<IdentityVerificationReadModel>.IndexKeys.Ascending(x => x.VerificationStatus),
                new CreateIndexOptions { Name = "idx_status" }));

        logger.LogInformation("Read database indexes created successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating read database indexes");
    }
}