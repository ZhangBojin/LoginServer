using LoginServer.Middleware.Consul;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConsulCenter>();

builder.Services.AddSingleton<ISqlSugarClient>(s =>
{
    var sqlSugarScope = new SqlSugarScope(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.MySql,
        ConnectionString = s.GetRequiredService<IConfiguration>().GetConnectionString("connectionString"),
        IsAutoCloseConnection = true,
    });
    return sqlSugarScope;
});

var app = builder.Build();

var task = app.Services.GetRequiredService<ConsulCenter>().ServiceRegistry();

//app.Services.GetRequiredService<ISqlSugarClient>().DbFirst.IsCreateAttribute().CreateClassFile("E:\\Csharp\\LoginServer\\Models", "Models");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
