using AutoMapper;
using CustomerManagementSystemV2.Dtos;
using CustomerManagementSystemV2.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CustomerManagementSystemV2.Services
{
    public  static class CustomerService
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder) 
        {
            builder.MapGet("/get-customers-list", async (IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer";
                    var customers = await connection.QueryAsync<Customer>(sql);
                    return Results.Ok(customers);
                }
            });

            builder.MapGet("/get-customer/{id}", async (int id, IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = $@" SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer WHERE Id = '{id}'";
                    var customers = await connection.QueryAsync<Customer>(sql);
                    return Results.Ok(customers);
                }
            });

            builder.MapPost("/create-customer", async (CustomerDto customerDto, IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = @" INSERT INTO Customer(FirstName, LastName, Email, DateOfBirth) VALUES (@FirstName, @LastName, @Email, @DateOfBirth)";
                    var parameters = new DynamicParameters();
                    parameters.Add("FirstName", customerDto.FirstName, DbType.String);
                    parameters.Add("LastName", customerDto.LastName, DbType.String);
                    parameters.Add("Email", customerDto.Email, DbType.String);
                    parameters.Add("DateOfBirth", customerDto.DateOfBirth, DbType.DateTime);
                    var customers = await connection.ExecuteAsync(sql, parameters);
                    return Results.Ok(customers);
                }
            });
        }
    }
}
