using AutoMapper;
using CustomerManagementSystemV2.Dtos;
using CustomerManagementSystemV2.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Data;

namespace CustomerManagementSystemV2.Services
{
    public  static class CustomerService
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder) 
        {
            builder.MapGet("/get-customers-list", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                //var connectionString = configuration.GetConnectionString("DBConnection");

                using (var connection = sqlConnectionFactory.CreateConnection())
                {
                    const string sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer";
                    IEnumerable<Customer> customers = await connection.QueryAsync<Customer>(sql);
                    return Results.Ok(customers);
                }
            });

            builder.MapGet("/get-customer/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                /*var connectionString = configuration.GetConnectionString("DBConnection");*/
                using (var connection = sqlConnectionFactory.CreateConnection())
                {
                    string sql = $@" SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer WHERE Id = '{id}'";
                    var customer = await connection.QueryAsync<Customer>(sql);
                    return customer is not null ? Results.Ok(customer) : Results.NotFound();
                }
            });

            builder.MapPost("/create-customer", async (CustomerDto customerDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                // var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = sqlConnectionFactory.CreateConnection())
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

            builder.MapPut("/update-customer/{id}", async (int id,CustomerDto customerDto, IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    string existing = $@" SELECT * FROM Customer WHERE Id = '{id}'";
                    var existingCustomers = await connection.QueryAsync<Customer>(existing);
                    if (existingCustomers.Count() == 0) 
                    { 
                        return Results.NotFound("This Id value is not found"); 
                    }

                    if (existingCustomers.Count() > 1)
                    {
                        return Results.BadRequest("This Id value found more than one");
                    }

                    string sql = $@" UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth WHERE Id = '{id}'";
                    var parameters = new DynamicParameters();
                    parameters.Add("FirstName", customerDto.FirstName, DbType.String);
                    parameters.Add("LastName", customerDto.LastName, DbType.String);
                    parameters.Add("Email", customerDto.Email, DbType.String);
                    parameters.Add("DateOfBirth", customerDto.DateOfBirth, DbType.DateTime);
                    var customers = await connection.ExecuteAsync(sql, parameters);
                    return Results.Ok(customers);
                }
            });

            builder.MapDelete("/delete-customer/{id}", async (int id, IConfiguration configuration) =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    string existing = $@" SELECT * FROM Customer WHERE Id = {id}";
                    var existingCustomers = await connection.QueryAsync(existing);

                    if (existingCustomers.Count() == 0) {

                        return Results.NotFound("This Id value is not found.");
                    }

                    if (existingCustomers.Count() > 1)
                    {
                        return Results.BadRequest("This Id value found more than one");
                    }

                    string sql = $@" DELETE FROM Customer WHERE Id = {id}";
                    var customers = await connection.ExecuteAsync(sql);
                    return Results.Ok(customers);
                }
            });
        }
    }
}
