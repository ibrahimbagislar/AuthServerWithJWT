using SharedLibrary.Configuration;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();

//Jwt Authentication Options
builder.Services.AddTokenAuthSharedLibrary(tokenOptions);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
