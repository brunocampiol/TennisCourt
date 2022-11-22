using Steeltoe.Extensions.Configuration.CloudFoundry;
using TennisCourt.Api.Configurations;
using TennisCourt.Api.Swagger;
using TennisCourt.Infra.CrossCutting.Commons.Extensions;
using TennisCourt.Infra.CrossCutting.Commons.Providers;
using TennisCourt.Infra.CrossCutting.IoC;
using TennisCourt.Infra.Data.Context;
using TennisCourt.Infra.Data.Context.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCloudFoundry();
//if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != Environments.Development)
//{
//    builder.UseCloudHosting();
//}

builder.Services.AddControllersWithViews((options) =>
{
    options.Conventions.Add(new ActionHidingConvention());
}).AddNewtonsoftJson();

if (builder.Environment.IsDevelopment())
{
    IMvcBuilder mvcBuilder = builder.Services.AddRazorPages();
    mvcBuilder.AddRazorRuntimeCompilation();
}

// Setup Options framework with DI - Required for PCF Steeltoe
builder.Services.AddOptions();

// Read from VCAP_SERVICES env variable and and generate keys to IConfiguration
builder.Services.ConfigureCloudFoundryOptions(builder.Configuration);

builder.Services.AddOptionsCloudFoundry<UserProvidedSettingsProvider>(builder.Configuration, "NsUserProvided");

// ASP.NET HttpContext dependency
builder.Services.AddHttpContextAccessor();

// .NET Native DI Abstraction
builder.Services.RegisterServices();

builder.Services.AddAutoMapperSetup();

builder.Services.AddDbContext<TennisCourtContext>();

// Swagger
builder.Services.AddSwaggerSetup(builder.Configuration);

//security
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
//TODO: verify migrations for mongoDb
//if (!builder.Environment.IsProduction()) app.ApplyMigrations();

//Security
app.ApplyMigrations();
//if (!builder.Environment.IsDevelopment())
//{
//    app.ApplyMigrations();
//    app.UseHsts();
//    app.Use(async (context, next) =>
//    {
//        context.Response.Headers.Add("X-Frame-Options", "DENY");
//        await next();
//    });
//}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json",
        name: "Tenis Court API v1");
});

//app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();


//app.UseAuthentication();
//app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    //requires that all controllers have at least regular IdentityProvider Policy
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
