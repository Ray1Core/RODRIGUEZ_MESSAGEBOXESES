namespace RODRIGUEZ_MESSAGEBOXE.Services
{
    /// <summary>
    /// Builds a human-readable INSERT statement string for teaching-demo purposes
    /// (Activity 6, and the Register page's preview alert). This text is only ever
    /// displayed to the user via a JS alert() -- it is never executed against a
    /// real database. The actual account creation goes through AccountController's
    /// with a salted+hashed password.
    /// </summary>
    public static class SqlPreviewHelper
    {
        public static string BuildUserInsertStatement(string name, string email, string gender, int age, string address, string username, string password)
        {
            static string Esc(string value) => (value ?? string.Empty).Replace("'", "''");

            return "INSERT INTO User (Name, Email, Gender, Age, Address, Username, Password) VALUES (" +
                $"'{Esc(name)}', '{Esc(email)}', '{Esc(gender)}', {age}, '{Esc(address)}', '{Esc(username)}', '{Esc(password)}')";
        }
    }
}
