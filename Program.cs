using LoginServer.Middleware.Consul;
using LoginServer.Middleware.Jwt;
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

builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddStackExchangeRedisCache(r =>
{
    r.Configuration = builder.Configuration.GetSection("redis:Key").Value;
});

builder.Services.AddAuthentication();

var app = builder.Build();

var task = app.Services.GetRequiredService<ConsulCenter>().ServiceRegistry();


//Map()、MapWhen()管道中增加分支，条件匹配就走分支，且不切换回主分支
//MapWhen()：按条件执行，MapWhen比Map处理范围更广
//UseWhen()：按条件执行，与MapWhen不同的是，UseWhen执行完后切回主分支!
app.UseWhen(context => context.Request.Path.StartsWithSegments("/Main/TestJwt")
    , applicationBuilder =>
    {
        applicationBuilder.UseMiddleware<JwtMiddleware>();
    });

#region 测试代码
//var count = app.Services.GetRequiredService<ISqlSugarClient>().Queryable<userinfo>().Count();
//Console.Write("");
//app.Services.GetRequiredService<ISqlSugarClient>().DbFirst.IsCreateAttribute().CreateClassFile("E:\\Csharp\\LoginServer\\Models", "Models");
//var token= app.Services.GetRequiredService<JwtHelper>().GenerateJwtToken("zbj123");
//var istoken = app.Services.GetRequiredService<JwtHelper>().ValidateJwtToken(token,"zbj123");
//var rz=Encoding.UTF8.GetString(app.Services.GetRequiredService<IDistributedCache>().Get("zbj")!);
//Console.Write("");
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
