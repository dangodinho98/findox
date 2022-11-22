using Findox.Api.Middlewares;
using Findox.Domain.Entities;
using Findox.Infra.Data.Repositories.User;
using Findox.Infra.IoC;
using Findox.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();
// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

if (isDevelopment)
    await CreateDatabaseAndSeedData();

app.Run();

//For local setup only
async Task CreateDatabaseAndSeedData()
{
    var adminRole = new Role(id: 1, name: Constants.Roles.Admin);
    var managerRole = new Role(id: 2, name: Constants.Roles.Manager);
    var regularRole = new Role(id: 3, name: Constants.Roles.Regular);

    var testUsers = new List<Account>
    {
        new Account
        {
            UserId = 1, Username = "Admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
            Roles = new List<Role>() { adminRole }
        },
        new Account
        {
            UserId = 2, Username = "Manager", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
            Roles = new List<Role>() { managerRole }
        },
        new Account
        {
            UserId = 3, Username = "Normal", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
            Roles = new List<Role>() { regularRole }
        }
    };

    using var scope = app.Services.CreateScope();
    var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();

    var user = await accountRepository.GetByUsernameAsync(testUsers[0].Username);
    if (user is null)
        testUsers.ForEach(async u => { await accountRepository.CreateAsync(u); });
}