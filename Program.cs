using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.SqlClient; // <-- Make sure this is added
using RODRIGUEZ_MESSAGEBOXE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

var app = builder.Build();

// ==========================================
// SQL SERVER CONNECTION & SEEDING
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
}

// 1. Initialize the SQL-backed user store
UserStore.Initialize(connectionString);

// 2. Seed initial data using raw SQL queries
SeedDatabase(connectionString);
// ==========================================

// Configure Forwarded Headers
var fho = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
fho.KnownNetworks.Clear();
fho.KnownProxies.Clear();
app.UseForwardedHeaders(fho);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

// ==========================================
// SEED DATABASE METHOD
// ==========================================
static void SeedDatabase(string connectionString)
{
    // Raw SQL Query: 
    // 1. Check if the 'admin' user already exists.
    // 2. If it does NOT exist, insert a default admin account.
    // 3. Also seeds a regular test user for good measure.
    string seedSql = @"
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
        BEGIN
            INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
            VALUES ('System Admin', 'admin@rodriguez.com', 'Other', 30, 'Main Office', 'admin', 'admin123', GETUTCDATE());
        END;

        IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'juandelacruz')
        BEGIN
            INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
            VALUES ('Juan Dela Cruz', 'juan@example.com', 'Male', 20, 'Manila, Philippines', 'juandelacruz', 'password123', GETUTCDATE());
        END;
    ";

    try
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand(seedSql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        Console.WriteLine("--> Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        // We catch the error so the app doesn't crash if the database or table hasn't been created yet.
        Console.WriteLine($"--> Error seeding database: {ex.Message}");
    }
}