using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Service;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.externalServices;
using WebApp.Middlewares;
using WebApp.ViewModelMappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UniversityContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("UniversityConnection"))
    );

builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<UniversityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
//could add here google twitter etc.

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(10);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/logout";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    opt.SlidingExpiration = true;
});


builder.Services.AddAuthorization();

builder.Services.AddMvc();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<IPasswordGenerator, PasswordGenerator>();
builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();

builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MapperViewModelProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();

app.UseDefaultPasswordMiddleware();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Account}/{action=Login}/{id:int?}");

app.Run();
