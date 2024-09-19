var builder = WebApplication.CreateBuilder(args);

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200") // Specify allowed origins
                     .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                     .AllowAnyHeader(); // Allow all headers
    });
});
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
                c.RoutePrefix = string.Empty; // הצב את Swagger UI בשורש האפליקציה
            });
}

     


// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Other middleware (like routing, authorization, etc.)
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseAuthorization();
app.MapControllers();
app.Run("http://0.0.0.0:8080"); // Ensure the app listens on 8080

