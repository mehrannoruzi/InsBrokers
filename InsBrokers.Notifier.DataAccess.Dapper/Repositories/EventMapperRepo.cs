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
    public class EventMapperRepo : IEventMapperRepo
    {
        private SqlConnection _sqlConnection;

        public EventMapperRepo(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("NotifierDbContext"));
        }


        public async Task<bool> AddAsync(EventMapper model)
        {
            try
            {
                model.InsertDateMi = DateTime.Now;
                model.InsertDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);

                await _sqlConnection.ExecuteSpCommandAsync<int>("[Notifier].[InsertEventMapper]",
                    new { EventMapper = model.ToTableValuedParameter("[dbo].[Tvp_EventMapper]") });

                return true;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                if (e.Message.Contains("unique index")) return true;
                else return false;
            }
        }

        public async Task<IEnumerable<EventMapper>> GetAsync(EventType eventType, int applicationId)
        {
            try
            {
                var query = "SELECT		* " +
                            "FROM		[Notifier].[EventMapper] e " +
                            "WHERE		e.ApplicationId = @ApplicationId AND e.Type = @EventType ";
                return await _sqlConnection.ExecuteQueryAsync<EventMapper>(query, new { ApplicationId = applicationId, EventType = (byte)eventType });
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }
    }
}