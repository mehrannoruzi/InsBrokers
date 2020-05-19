using Elk.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [NotMapped]
    public class MenuSPModel
    {
        public int? ParentId { get; set; }

        public bool ShowInMenu { get; set; }

        public byte OrderPriority { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int RoleId { get; set; }

        public string RoleNameFa { get; set; }

        public bool IsDefault { get; set; }

        [NotMapped]
        public bool IsAction { get { return !string.IsNullOrWhiteSpace(ControllerName) && !string.IsNullOrWhiteSpace(ActionName); } }

        public bool HasChild { get { return !string.IsNullOrWhiteSpace(Actions); } }

        public string Actions { get; set; }

        [NotMapped]
        public List<MenuSPModel> ActionsList { get { return (Actions ?? "[]").DeSerializeJson<List<MenuSPModel>>(); } }

    }
}