using DataLayer;
using ServiceLayer.Core.IRepositories;
using ServiceLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SaniDBEntities _context;
        public UnitOfWork(SaniDBEntities context)
        {
            _context = context;
            Services = new ServiceRepository(_context);
            ServiceCategoryes = new ServiceCategoryRepository(_context);
            ServiceAssign = new ServiceAssignRepository(_context);
            Factor = new FactorRepository(_context);
            Branches = new BranchRepository(_context);
            Oprator = new OpratorRepository(_context);
            ElectricShops = new ElectricShopRepository(_context);
            Customers = new CustomerRepository(_context);
            CustomerAddress = new CustomerAddressRepository(_context);
            AssignToFactor = new AssignToFactorRepository(_context);
            Force = new ForceRepository(_context);
            ForceRating = new ForceRatingRepository(_context);
            ForceCancels = new ForceCancelsRepository(_context);
            ForceExp = new ForceExpertiseRepository(_context);
            Documents = new DocumentsRepository(_context);
            StateCity = new StateCityRepository(_context);
        }

        public IServiceRepository Services { get; private set; }
        public IServiceCategoryRepository ServiceCategoryes { get; private set; }
        public IServiceAssignRepository ServiceAssign { get; private set; }

        public IBranchRepository Branches { get; private set; }

        public IOpratorRepository Oprator { get; set; }

        public IElectricShopRepository ElectricShops { get; private set; }

        public ICustomerRepository Customers { get; private set; }
        public ICustomerAddressRepository CustomerAddress { get; set; }

        public IFactorRepository Factor { get; set; }
        public IAssignToFactorRepository AssignToFactor { get; private set; }

        public IForceRepository Force { get; set; }
        public IForceRatingRepository ForceRating { get; set; }
        public IForceCancelsRepository ForceCancels { get; set; }
        public IForceExpertiseRepository ForceExp { get; set; }

        public IDocumentsRepository Documents { get; set; }

        public IStateCityRepository StateCity { get; set; }

        public int Complete() => _context.SaveChanges();
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
