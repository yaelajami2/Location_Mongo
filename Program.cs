var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware ל-Swagger
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
        builder.WithOrigins( "http://localhost:4200") // URL של האפליקציה שלך
               .AllowAnyMethod() // התיר כל שיטת HTTP (GET, POST, PUT, DELETE וכו')
               .AllowAnyHeader(); // התיר כל כותרת בבקשות
    });
});
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
