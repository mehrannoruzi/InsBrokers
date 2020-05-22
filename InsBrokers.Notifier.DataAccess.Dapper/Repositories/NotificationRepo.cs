using System;
using Elk.Core;
using Elk.Dapper;
using System.Linq;
using InsBrokers.Domain;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace InsBrokers.Notifier.DataAccess.Dapper
{
    public class NotificationRepo : INotificationRepo
    {
        private SqlConnection _sqlConnection;

        public NotificationRepo(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("NotifierDbContext"));
        }


        public async Task<bool> AddAsync(Notification model)
        {
            try
            {
                model.InsertDateMi = DateTime.Now;
                model.InsertDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);

                await _sqlConnection.ExecuteSpCommandAsync<int>("[Notifier].[InsertNotification]",
                    new { Notification = model.ToTableValuedParameter("[dbo].[Tvp_Notification]") });

                return true;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                if (e.Message.Contains("unique index")) return true;
                else return false;
            }
        }

        public async Task<bool> UpdateAsync(UpdateNotificationDto model)
        {
            try
            {
                await _sqlConnection.ExecuteSpCommandAsync<int>("[Notifier].[UpdateNotification]",
                    new { NotificationId = model.NotificationId, Status = model.Status, SendStatus = model.SendStatus, SendDateMi = model.SendDateMi , IsLock =model.IsLock});

                return true;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return false;
            }
        }

        public async Task<Notification> FindAsync(int notificationId)
        {
            try
            {
                var query = "SELECT * " +
                            "FROM   [Notifier].[Notification] " +
                            "WHERE  [NotificationId] = @NotificationId";
                return await _sqlConnection.ExecuteQuerySingleAsync<Notification>(query, new { NotificationId = notificationId });
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }

        public async Task<IEnumerable<Notification>> GetAsync(NotificationStatus status, PagingParameter pagingParameter)
        {
            try
            {
                var query = "SELECT		* " +
                            "FROM		[Notifier].[Notification] n " +
                            "WHERE      n.Status = @Status " +
                            "ORDER BY	p.NotificationId ASC " +
                            "OFFSET		@PageSize * (@PageNumber - 1) ROWS " +
                            "FETCH NEXT	@PageSize ROWS ONLY;";
                return await _sqlConnection.ExecuteQueryAsync<Notification>(query,
                    new { Status = status, pagingParameter.PageNumber, pagingParameter.PageSize });
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }

        public async Task<IEnumerable<Notification>> GetUnProccessAsync()
        {
            try
            {
                var notifications = await _sqlConnection.ExecuteSpCommandAsync<Notification>("[Notifier].[GetUnProccessNotification]");

                if (notifications.Any()) return notifications;
                return new List<Notification>();
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return new List<Notification>();
            }
        }
    }
}