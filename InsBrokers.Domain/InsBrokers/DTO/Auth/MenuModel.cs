using Elk.Core;
using System.Linq;
using System.Collections.Generic;

namespace InsBrokers.Domain
{
    public class MenuModel
    {
        public string Menu { get; set; }
        public UserAction DefaultUserAction { get; set; }
        public IEnumerable<UserAction> ActionList { get; set; }
        public Dictionary<int, string> Roles { get => ActionList.Select(x => new { x.RoleId, x.RoleNameFa }).Distinct().ToDictionary(x => x.RoleId, x => x.RoleNameFa); }
    }
}