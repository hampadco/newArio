

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<Context>(x=>
x.UseSqlServer(builder.Configuration.GetConnectionString("Default")));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5014") // 🚀 آدرس دقیق فرانت‌اند را وارد کن
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // باعث می‌شود Swagger در روت نمایش داده شود
});




app.MapControllers();
app.Run();

