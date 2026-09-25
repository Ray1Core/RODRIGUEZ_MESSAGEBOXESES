using Microsoft.Data.SqlClient;
using System.Data;

namespace RODRIGUEZ_MESSAGEBOXE.Models
{
    public static class UserStore
    {
        private static string _connectionString = string.Empty;

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        // VIEW INFORMATION (Read)
        public static List<User> GetAll()
        {
            var users = new List<User>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc FROM Users ORDER BY Id DESC";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(MapReaderToUser(reader));
                    }
                }
            }
            return users;
        }

        public static User? FindById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc FROM Users WHERE Id = @Id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapReaderToUser(reader);
                    }
                }
            }
            return null;
        }

        public static User? FindByUsername(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc FROM Users WHERE Username = @Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapReaderToUser(reader);
                    }
                }
            }
            return null;
        }

        public static User? FindByEmail(string email)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc FROM Users WHERE Email = @Email";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapReaderToUser(reader);
                    }
                }
            }
            return null;
        }

        // CREATE INFORMATION (Insert)
        public static User Add(User user)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc) 
                               OUTPUT INSERTED.Id 
                               VALUES (@Name, @Email, @Gender, @Age, @Address, @Username, @Password, @CreatedAtUtc)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Age", user.Age);
                    cmd.Parameters.AddWithValue("@Address", user.Address ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@CreatedAtUtc", user.CreatedAtUtc);

                    user.Id = (int)cmd.ExecuteScalar();
                }
            }
            return user;
        }

        // UPDATE INFORMATION
        public static void Update(User user)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"UPDATE Users 
                               SET Name = @Name, Email = @Email, Gender = @Gender, Age = @Age, 
                                   Address = @Address, Username = @Username, Password = @Password 
                               WHERE Id = @Id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Age", user.Age);
                    cmd.Parameters.AddWithValue("@Address", user.Address ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE INFORMATION
        public static void Remove(User user)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Users WHERE Id = @Id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Gender = reader["Gender"].ToString() ?? string.Empty,
                Age = reader["Age"] != DBNull.Value ? (int)reader["Age"] : 0,
                Address = reader["Address"].ToString() ?? string.Empty,
                Username = reader["Username"].ToString() ?? string.Empty,
                Password = reader["Password"].ToString() ?? string.Empty,
                CreatedAtUtc = (DateTime)reader["CreatedAtUtc"]
            };
        }
    }
}