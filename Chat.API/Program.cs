using Chat.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(CorsOptions =>
{
    CorsOptions.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection(nameof(TwilioSettings)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.MapControllers();

app.Run();