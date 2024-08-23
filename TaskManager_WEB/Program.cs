using Microsoft.AspNetCore.Authentication.Cookies;
using TaskManager_WEB.AutoMapper;
using TaskManager_WEB.Services;
using TaskManager_WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    opt.LoginPath = "/auth/login";
    opt.AccessDeniedPath = "/home/AccessDenied";
    opt.SlidingExpiration = true;
});

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(15);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
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
    pattern: "Staff List",
    defaults: new { controller = "User", action = "UserList" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=user}/{action=getallusers}/{id?}"
);

app.Run();
