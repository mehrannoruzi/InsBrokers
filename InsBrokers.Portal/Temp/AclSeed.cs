using System;
using Elk.Core;
using System.Linq;
using System.Collections.Generic;
using InsBrokers.DataAccess.Ef;
using InsBrokers.Domain;

namespace InsBrokers.Portal
{
    public class AclSeed
    {
        private readonly AuthDbContext _db;
        private readonly AppDbContext _appDb;

        public AclSeed(AuthDbContext db, AppDbContext appDb)
        {
            _db = db;
            _appDb = appDb;
        }

        /// <summary>
        /// initializing Acl tables, remove after using
        /// for using in Hillavas.BaseProject_MaterialPro.Data.Migrations.Configuration.cs
        /// just add "AclSeed.Init(context);" to seed method
        /// </summary>
        /// <param name="db">project context</param>
        public bool Init()
        {
            using (var bt = _db.Database.BeginTransaction())
            {
                var rep = AddAdminUserAndRole();
                if (!rep.Success) return false;

                #region UserInRole
                var userInRole = new UserInRole()
                {
                    UserId = rep.UserId,
                    RoleId = rep.RoleId
                };
                _db.Set<UserInRole>().Add(userInRole);

                if (_db.SaveChanges() == 0)
                {
                    bt.Rollback();
                    return false;
                }
                #endregion

                var actionsRep = AddActions();
                if (!actionsRep.Success)
                {
                    bt.Rollback();
                    return false;
                }
                _db.Set<ActionInRole>().AddRange(actionsRep.actions.Select(x => new ActionInRole
                {
                    RoleId = rep.RoleId,
                    ActionId = x.ActionId,
                    IsDefault = x.ActionName == "ProfileInfo" ? true : false
                }));
                if (_db.SaveChanges() == 0)
                {
                    bt.Rollback();
                    return false;
                }
                bt.Commit();
                return true;
            }
        }

        public (bool Success, int RoleId, Guid UserId) AddAdminUserAndRole()
        {
            var role = new Role()
            {
                RoleNameEn = "Admin",
                RoleNameFa = "مدیر سیستم",
                Enabled = true
            };
            _db.Set<Role>().Add(role);
            var addRole = _db.SaveChanges();
            if(addRole == 0) return (false, 0, Guid.Empty);
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Name = "مدیر",
                Family = "سیستم",
                Email = "admin@hillavas.com",
                Password = HashGenerator.Hash("@dmin"),
                MobileNumber = 91212345678,
                IsActive = true,
                NationalCode = "5160000608",
                FatherName ="father",
                BaseInsurance = BaseInsuranceType.BeduneBimehPayeh,
                LastLoginDateSh = "1397/06/04",
                LastLoginDateMi = new DateTime(2018, 8, 26),
                InsertDateSh = "1397/06/04",
                InsertDateMi = new DateTime(2018, 8, 26)
            };
            _appDb.Set<User>().Add(user);
            var res = _appDb.SaveChanges();
            if (res == 0) return (false, 0, Guid.Empty);
            else return (true, role.RoleId, user.UserId);
        }

        public (bool Success, List<Domain.Action> actions) AddActions()
        {
            var ActionList = new List<Domain.Action> {
                new Domain.Action()
                {
                    Name = "پروفایل",
                    ControllerName = "User",
                    ActionName = "ProfileInfo",
                    Icon = "zmdi zmdi-account",
                    OrderPriority = 1,
                    ShowInMenu = true,
                }
            };

            #region Acl Actions
            var parent = new Domain.Action()
            {
                Name = "سطوح دسترسی",
                Icon = "zmdi zmdi-assignment-account",
                OrderPriority = 50,
                ShowInMenu = true,
            };
            _db.Set<Domain.Action>().Add(parent);
            if (_db.SaveChanges() == 0) return (false, null);

            #region Users Actions
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "مدیریت کاربران",
                ControllerName = "User",
                ActionName = "Manage",
                Icon = "zmdi zmdi-accounts",
                OrderPriority = 51,
                ShowInMenu = true,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "افزودن کاربر",
                ControllerName = "User",
                ActionName = "Add",
                Icon = "zmdi zmdi-account-add",
                OrderPriority = 52,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "ویرایش کاربر",
                ControllerName = "User",
                ActionName = "Update",
                Icon = "zmdi zmdi-edit",
                OrderPriority = 53,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "حذف کاربر",
                ControllerName = "User",
                ActionName = "Delete",
                OrderPriority = 54,
                Icon = "zmdi zmdi-delete",
                ShowInMenu = false,
            });
            #endregion
            #region Actions
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "مدیریت اکشن ها",
                ControllerName = "Action",
                ActionName = "Manage",
                Icon = "zmdi zmdi-view-list",
                OrderPriority = 55,
                ShowInMenu = true,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "افزودن اکشن",
                ControllerName = "Action",
                ActionName = "Add",
                Icon = "zmdi zmdi-account-add",
                OrderPriority = 57,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "ویرایش اکشن",
                ControllerName = "Action",
                ActionName = "Update",
                Icon = "zmdi zmdi-edir",
                OrderPriority = 58,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "حذف اکشن",
                ControllerName = "Action",
                ActionName = "Delete",
                Icon = "zmdi zmdi-account-add",
                OrderPriority = 58,
                ShowInMenu = false,
            });
            #endregion
            #region ActionInRole Actions
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "افزودن اکشن به نقش",
                ControllerName = "ActionInRole",
                ActionName = "Add",
                Icon = "zmdi zmdi-collection-plus",
                OrderPriority = 59,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "حذف اکشن از نقش",
                ControllerName = "ActionInRole",
                ActionName = "Delete",
                Icon = "zmdi zmdi-delete",
                OrderPriority = 60,
                ShowInMenu = false,
            });
            #endregion
            #region Role Actions
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "مدیریت نقش ها",
                ControllerName = "Role",
                ActionName = "Manage",
                Icon = "zmdi zmdi-accounts-list",
                OrderPriority = 61,
                ShowInMenu = true,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "افزودن نقش",
                ControllerName = "Role",
                ActionName = "Add",
                OrderPriority = 62,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "ویرایش نقش",
                ControllerName = "Role",
                ActionName = "Update",
                OrderPriority = 63,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "حذف نقش",
                ControllerName = "Role",
                ActionName = "Delete",
                OrderPriority = 64,
                ShowInMenu = false,
            });
            #endregion
            #region UserInRole Actions
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "افزودن نقش به کاربر",
                ControllerName = "UserInRole",
                ActionName = "Add",
                Icon = "zmdi zmdi-collection-plus",
                OrderPriority = 59,
                ShowInMenu = false,
            });
            ActionList.Add(new Domain.Action()
            {
                ParentId = parent.ActionId,
                Name = "حذف نقش از کاربر",
                ControllerName = "UserInRole",
                ActionName = "Delete",
                Icon = "zmdi zmdi-delete",
                OrderPriority = 60,
                ShowInMenu = false,
            });

            #endregion
            #endregion

            _db.Set<Domain.Action>().AddRange(ActionList);
            ActionList.Insert(0, parent);
            if (_db.SaveChanges() == 0)
                return (false, null);
            else
                return (true, ActionList);
        }
    }
}
