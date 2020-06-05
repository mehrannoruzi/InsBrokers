using System;

namespace InsBrokers.InfraStructure
{
    public static class GlobalVariables
    {
        public static class CacheSettings
        {
            public static string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";
            public static string MainMenuCacheKey(Guid userId) => $"MainMenu_{userId.ToString().Replace("-", "_")}";

            
        }

        public static class SmsProviders
        {
            public static class LinePayamak
            {
                public static string Username = "500096998998";
                public static string Password = "80225353";
                public static string SenderId = "500096998998";
            }
        }
    }
}