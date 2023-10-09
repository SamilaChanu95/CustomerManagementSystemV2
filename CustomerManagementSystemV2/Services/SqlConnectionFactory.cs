using Microsoft.Data.SqlClient;

namespace CustomerManagementSystemV2.Services
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection() 
        {
            return new SqlConnection(_connectionString);
        }
    }
}
