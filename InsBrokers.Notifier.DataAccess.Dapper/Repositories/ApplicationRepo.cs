using System;
using Elk.Core;
using Elk.Dapper;
using InsBrokers.Domain;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace InsBrokers.Notifier.DataAccess.Dapper
{
    public class ApplicationRepo : IApplicationRepo
    {
        private SqlConnection _sqlConnection;

        public ApplicationRepo(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("NotifierDbContext"));
        }


        public async Task<bool> AddAsync(Application model)
        {
            try
            {
                model.InsertDateMi = DateTime.Now;
                model.InsertDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);

                await _sqlConnection.ExecuteSpCommandAsync<int>("[Notifier].[InsertApplication]",
                    new { EventMapper = model.ToTableValuedParameter("[dbo].[Tvp_Application]") });

                return true;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                if (e.Message.Contains("unique index")) return true;
                else return false;
            }
        }

        public async Task<Application> GetAsync(Guid token)
        {
            var query = "SELECT		* " +
                        "FROM		[Notifier].[Application] a " +
                        "WHERE      a.IsActive = 1 AND a.Token = @Token";
            return await _sqlConnection.ExecuteQuerySingleAsync<Application>(query, new { Token = token });
        }

        public async Task<IEnumerable<Application>> GetAsync(PagingParameter pagingParameter)
        {
            try
            {
                var query = "SELECT		* " +
                            "FROM		[Notifier].[Application] n " +
                            "ORDER BY	p.ApplicationId ASC " +
                            "OFFSET		@PageSize * (@PageNumber - 1) ROWS " +
                            "FETCH NEXT	@PageSize ROWS ONLY;";
                return await _sqlConnection.ExecuteQueryAsync<Application>(query,
                    new { pagingParameter.PageNumber, pagingParameter.PageSize });
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }
    }
}