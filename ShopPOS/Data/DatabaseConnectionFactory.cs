using System.Configuration;
using MySql.Data.MySqlClient;

namespace ShopPOS.Data
{
    public static class DatabaseConnectionFactory
    {
        public static MySqlConnection CreateOpenConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["ShopPosDb"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException("Database connection string 'ShopPosDb' is missing from App.config.");
            }

            MySqlConnection connection = new MySqlConnection(settings.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
