using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSingleton<bankApi.Repositories.IAccountRepository, bankApi.Repositories.AccountRepository>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Bank", 
        Version = "v1" 
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank V1");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
