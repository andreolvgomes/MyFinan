using FastEndpoints;
using MyFinan;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddFastEndpoints();
builder.Services.AddConfigs();

var app = builder.Build();

app.UseFastEndpoints();
app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");
app.MapGet("/warmpup", () => "WarmUp OK");

app.Run();
