using Microsoft.Data.SqlClient;
using StudentTeacherAPI.Models;

namespace StudentTeacherAPI.DAL
{
    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // REGISTER a new user
        public void RegisterUser(User user)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "INSERT INTO Users (Name, DOB, Designation, Email, Password) VALUES (@Name, @DOB, @Designation, @Email, @Password)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@DOB", user.DOB);
            cmd.Parameters.AddWithValue("@Designation", user.Designation);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // GET user by Email (for login)
        public User? GetUserByEmail(string email)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = (int)reader["UserId"],
                    Name = reader["Name"].ToString()!,
                    DOB = (DateTime)reader["DOB"],
                    Designation = reader["Designation"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Password = reader["Password"].ToString()!
                };
            }
            return null;
        }
    }
}