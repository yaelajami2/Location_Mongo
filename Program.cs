var builder = WebApplication.CreateBuilder(args);

// ����� ������ Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware �-Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

     
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins( "http://localhost:4200") // URL �� ��������� ���
               .AllowAnyMethod() // ���� �� ���� HTTP (GET, POST, PUT, DELETE ���')
               .AllowAnyHeader(); // ���� �� ����� ������
    });
});
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
