var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr(); // +
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://localhost:5000");

app.UseAuthorization();

app.MapControllers();

app.UseCloudEvents(); // +
app.MapSubscribeHandler(); // +

app.Run();
