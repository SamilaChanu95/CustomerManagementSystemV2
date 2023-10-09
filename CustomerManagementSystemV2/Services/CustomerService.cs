using CustomerManagementSystemV2.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CustomerManagementSystemV2.Services
{
    public  static class CustomerService
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder) 
        {
            builder.MapGet("customers", async (IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer";
                    var customers = await connection.QueryAsync<Customer>(sql);
                    return Results.Ok(customers);
                }
            });
        }
    }
}
