using System;
using Elk.Core;
using System.Threading;
using InsBrokers.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace InsBrokers.DataAccess.Ef
{
    public sealed class AppUnitOfWork : IElkUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IServiceProvider _serviceProvider;

        public AppUnitOfWork(IServiceProvider serviceProvider, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
        }


        #region Base
        public IUserRepo UserRepo => _serviceProvider.GetService<IUserRepo>();
        //public IGenericRepo<Role> RoleRepo=> _serviceProvider.GetService<IGenericRepo<Role>>();
        //public IGenericRepo<UserInRole> UserInRoleRepo=> _serviceProvider.GetService<IGenericRepo<UserInRole>>();
        public IGenericRepo<Address> AddressRepo=> _serviceProvider.GetService<IGenericRepo<Address>>();
        public IGenericRepo<ContactUs> ContactUsRepo => _serviceProvider.GetService<IGenericRepo<ContactUs>>();
        public IGenericRepo<Relative> RelativeRepo => _serviceProvider.GetService<IGenericRepo<Relative>>();
        public IGenericRepo<BankAccount> BankAccountRepo=> _serviceProvider.GetService<IGenericRepo<BankAccount>>();
        #endregion

        #region Insurance
        public ILossRepo LossRepo => _serviceProvider.GetService<ILossRepo>();
        public IGenericRepo<LossAsset> LossAssetRepo => _serviceProvider.GetService<IGenericRepo<LossAsset>>(); 
        #endregion


        public ChangeTracker ChangeTracker { get => _appDbContext.ChangeTracker; }
        public DatabaseFacade Database { get => _appDbContext.Database; }

        public SaveChangeResult ElkSaveChanges()
            => _appDbContext.ElkSaveChanges();

        public Task<SaveChangeResult> ElkSaveChangesAsync(CancellationToken cancellationToken = default)
            => _appDbContext.ElkSaveChangesAsync(cancellationToken);

        public SaveChangeResult ElkSaveChangesWithValidation()
            => _appDbContext.ElkSaveChangesWithValidation();

        public Task<SaveChangeResult> ElkSaveChangesWithValidationAsync(CancellationToken cancellationToken = default)
            => _appDbContext.ElkSaveChangesWithValidationAsync(cancellationToken);

        public int SaveChanges()
            => _appDbContext.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _appDbContext.SaveChangesAsync(cancellationToken);

        public Task<int> SaveChangesAsync(bool AcceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => _appDbContext.SaveChangesAsync(AcceptAllChangesOnSuccess, cancellationToken);

        public void Dispose() => _appDbContext.Dispose();
    }
}