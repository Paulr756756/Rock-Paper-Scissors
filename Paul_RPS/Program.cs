using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Paul_RPS;
using Paul_RPS.Data;

var builder = WebApplication.CreateBuilder(args);

DependencyExtensions.RegisterStandardDependencies(builder);
DependencyExtensions.RegisterBusinessDependencies(builder);

var app = builder.Build();

DependencyExtensions.UseDefaultBuilderMethods(app);

app.Run();
