using System;

namespace InsBrokers.InfraStructure
{
    public static class GlobalVariables
    {
        public static class CacheSettings
        {
            public static string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";
        }
    }
}
