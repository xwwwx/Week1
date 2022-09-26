using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using W1.Server;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq(context.Configuration.GetValue<string>("Seq:Url")));

// Add services to the container.
builder.Services.AddDbContext<W1DbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("W1"));
});

builder.Services.AddSingleton<W1DapperContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
