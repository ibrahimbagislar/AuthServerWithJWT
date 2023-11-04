using AuthServer.Core.Configuration;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.DataAccess.Contexts;
using AuthServer.DataAccess.Repositories;
using AuthServer.DataAccess.UnitOfWork;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Services;
using SharedLibrary.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


// IoC Container Register (doðrusu her katmanýn kendisine ait bir serviceregistiration'ý olmasý ancak proje büyük olmadýðýndan bu yöntem seçildi.)
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//dbcontext
builder.Services.AddDbContext<AuthServerAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQL"), opt =>
    {
        opt.MigrationsAssembly("AuthServer.DataAccess");
    });
});
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<AuthServerAppDbContext>();

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

//Authentication Ekleme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudiences = tokenOptions.Audience,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

////fluentvalidations ekleme
//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers().AddFluentValidation(opt =>
{
     opt.RegisterValidatorsFromAssemblyContaining<Program>();
});
//fluentvalidationdan dönen hata mesajlarýný kendi istediðimiz dto'ya çevirme extension metot
builder.Services.AddCustomValidationResponse();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
{
    app.UseCustomException();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
