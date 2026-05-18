using FastEndpoints;
using MyFinan;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddFastEndpoints();
builder.Services.AddConfigs();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseFastEndpoints();
app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");
app.MapGet("/warmpup", () => "WarmUp OK");

app.Run();
