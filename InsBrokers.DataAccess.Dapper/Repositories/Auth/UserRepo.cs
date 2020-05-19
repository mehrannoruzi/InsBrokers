using System;
using Elk.Dapper;
using InsBrokers.Domain;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace InsBrokers.DataAccess.Dapper
{
    public class DapperUserRepo
    {
        private SqlConnection _sqlConnection;

        public DapperUserRepo(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("AppDbContext"));
        }


        public IEnumerable<MenuSPModel> GetUserMenu(Guid userId)
           => _sqlConnection.ExecuteSpList<MenuSPModel>("[Auth].[GetUserMenu]", new { UserId = userId }); 
    }
}