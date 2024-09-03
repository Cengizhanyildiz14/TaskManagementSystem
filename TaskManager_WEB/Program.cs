using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using TaskManager_WEB.AutoMapper;
using TaskManager_WEB.Resources;
using TaskManager_WEB.Services;
using TaskManager_WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSingleton<LanguageService>();

builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportCulture = new List<CultureInfo>
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US")
    };
    opt.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
    opt.SupportedCultures = supportCulture;
    opt.SupportedUICultures = supportCulture;

    opt.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IK", policy =>
    policy.RequireClaim("DepartmentName", "Ýnsan Kaynaklarý Uzmaný"));
});

builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddHttpClient<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    opt.LoginPath = "/auth/login";
    opt.AccessDeniedPath = "/home/AccessDenied";
    opt.SlidingExpiration = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/NotFoundPage");

app.MapControllerRoute(
    name: "profile",
    pattern: "Profile/{id?}",
    defaults: new { controller = "User", action = "Profile" }
);

app.MapControllerRoute(
    name: "TaskCreate",
    pattern: "TaskCreate",
    defaults: new { controller = "Task", action = "Create" }
);

app.MapControllerRoute(
    name: "MyTasks",
    pattern: "UsersTask/{id?}",
    defaults: new { controller = "User", action = "UsersTask" }
);

app.MapControllerRoute(
    name: "TaskDetail",
    pattern: "TaskDetail/{id?}",
    defaults: new { controller = "Task", action = "TaskDetails" }
);

app.MapControllerRoute(
    name: "TaskUpdate",
    pattern: "TaskUpdate/{id?}",
    defaults: new { controller = "Task", action = "TaskUpdate" }
);

app.MapControllerRoute(
    name: "Privacy",
    pattern: "Privacy",
    defaults: new { controller = "Home", action = "Privacy" }
);

app.MapControllerRoute(
    name: "DepartmentCreate",
    pattern: "DepartmentCreate",
    defaults: new { controller = "Department", action = "DepartmentCreate" }
);

app.MapControllerRoute(
    name: "UserCreate",
    pattern: "UserCreate",
    defaults: new { controller = "User", action = "UserCreate" }
);

app.MapControllerRoute(
    name: "UserList",
    pattern: "StaffList",
    defaults: new { controller = "User", action = "UserList" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=home}/"
);

app.Run();
