using System.Text;
using GestorOpiniones.Api.Settings;
using GestorOpiniones.Api.Middleware;
using GestorOpiniones.Api.Repositories;
using GestorOpiniones.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;


// Me costo hacer esto profe porfaxd espero lea esto, saludos!

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

var jwt = builder.Configuration.GetSection("JwtSettings");
var secret = jwt["SecretKey"] ?? throw new InvalidOperationException("Jwt SecretKey not set");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

var mongo = builder.Configuration.GetSection("MongoSettings");
var connection = mongo["ConnectionString"] ?? "mongodb://localhost:27017";
var databaseName = mongo["DatabaseName"] ?? "gestor_opiniones";
var client = new MongoClient(connection);
builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

// Repositories & services
builder.Services.AddSingleton<PostRepository>();
builder.Services.AddSingleton<CommentRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
