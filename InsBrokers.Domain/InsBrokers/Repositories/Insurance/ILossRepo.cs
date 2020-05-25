using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InsBrokers.Domain
{
    public interface ILossRepo : IGenericRepo<Loss>, IScopedInjection
    {
        Task<Dictionary<string, int>> GetLossCountLastDaysAsync(int dayCount = 10);
    }
}