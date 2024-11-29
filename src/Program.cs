using Microsoft.Extensions.Options;
using TestLdapAuthApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LdapSettings>(builder.Configuration.GetSection("LdapSettings"));

builder.Services.AddScoped<LdapAuthenticationService>(provider =>
{
    var ldapSettings = provider.GetRequiredService<IOptions<LdapSettings>>().Value;
    return new LdapAuthenticationService(ldapSettings.ServerAddress, ldapSettings.BaseDn);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/authenticate", (LdapAuthenticationService ldapService, LoginModel model) =>
{
    if (ldapService.Authenticate(model.Username, model.Password))
    {
        return Results.Ok(new { message = "Authentication successful" });
    }
    else
    {
        return Results.Unauthorized();
    }
})
.WithName("AuthenticateUser")
.WithOpenApi();

app.Run();