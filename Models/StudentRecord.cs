namespace RODRIGUEZ_MESSAGEBOXE.Models
{
    /// <summary>
    /// Mutable record used by the Activity 9 / 10 real in-memory CRUD demos.
    /// Not connected to any database â€” data resets on app restart.
    /// </summary>
    public class StudentRecord
    {
        public int    Id       { get; set; }
        public string Name     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
