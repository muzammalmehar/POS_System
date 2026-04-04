using System;
using System.Data;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;
using ShopPOS.Security;

namespace ShopPOS.Services
{
    public class AuthService
    {
        public UserSession Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        u.user_id,
                        u.full_name,
                        u.username,
                        u.password_hash,
                        r.role_name
                    FROM users u
                    INNER JOIN roles r ON r.role_id = u.role_id
                    WHERE u.username = @username
                        AND u.is_active = 1";

                command.Parameters.AddWithValue("@username", username.Trim());

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    string storedPassword = Convert.ToString(reader["password_hash"]);
                    if (!PasswordHasher.Verify(password, storedPassword))
                    {
                        return null;
                    }

                    return new UserSession
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FullName = Convert.ToString(reader["full_name"]),
                        Username = Convert.ToString(reader["username"]),
                        RoleName = Convert.ToString(reader["role_name"])
                    };
                }
            }
        }

        public bool CanConnect()
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                return connection.State == ConnectionState.Open;
            }
        }
    }
}
