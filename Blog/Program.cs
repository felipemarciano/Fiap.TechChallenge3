using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Blog.Configuration;
using Blog.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpClient<AuthenticationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["JwtSettings:Api"] ?? "");
});
builder.Services.AddHttpClient<ProfileService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["JwtSettings:Api"] ?? "");
});
builder.Services.AddHttpClient<BlogPostService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["JwtSettings:Api"] ?? "");
});

builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("AzureBlobStorage"));
builder.Services.AddSingleton<GlobalState>();
builder.Services.AddSingleton<BlobStorageService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "auth_cookie";
})
.AddJwtBearer();

builder.Services.AddMudServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
