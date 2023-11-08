﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Assignment1v3.Data;
using Microsoft.AspNetCore.Identity;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
StripeConfiguration.ApiKey = "sk_test_51Ny4RlB0mWSJyvtEF6kjAFRYirP53gUXjltsjcI2iMjbQmedAr1zFvqBa9TdFDQ7crej0BI1dEK5r5l21k64Jf1L00G9IYoBmH";


builder.Services.AddDbContext<Assignment1v3Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Assignment1v3Context") ?? throw new InvalidOperationException("Connection string 'Assignment1v3Context' not found.")));
;
builder.Services.AddAuthentication("AuthCookie").AddCookie("AuthCookie", options =>
{
    options.Cookie.Name = "AuthCookie";
    options.LoginPath = "/Account/Login";
}) ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeStudent",
        policy => policy.RequireClaim("Role", "Student"));
    options.AddPolicy("MustBeInstructor",
        policy => policy.RequireClaim("Role", "Instructor"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {  endpoints.MapRazorPages(); });

app.MapRazorPages();




app.Run();
