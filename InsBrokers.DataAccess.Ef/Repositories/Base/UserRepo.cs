using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InsBrokers.DataAccess.Ef
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        private readonly AppDbContext _appContext;

        public UserRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public async Task<User> FindByMobileNumber(long mobileNumber) => await FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber);

        public async Task<int> GetUserCount() => await _dbSet.CountAsync();

        public async Task<Dictionary<string, int>> GetUserCountLastDaysAsync(int dayCount = 10)
        {
            var fromDate = DateTime.Now.AddDays(-dayCount);
            var result = new Dictionary<string, int>();

            var userCount = await _dbSet.AsNoTracking().Where(x => x.InsertDateMi >= fromDate)
                .GroupBy(x => x.InsertDateSh)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToListAsync();

            for (int i = dayCount - 1; i >= 0; i--)
            {
                var date = PersianDateTime.Parse(DateTime.Now.AddDays(-i)).ToString(PersianDateTimeFormat.Date);
                var statistic = userCount.FirstOrDefault(x => x.Date == date);
                if (statistic != null) result.Add(date, statistic.Count);
                else result.Add(date, 0);
            }

            return result;
        }
    }
}
