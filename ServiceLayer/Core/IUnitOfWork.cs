using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository Services { get; }

        IServiceCategoryRepository ServiceCategoryes { get; }

        IServiceAssignRepository ServiceAssign { get; }

        IBranchRepository Branches { get; }

        IOpratorRepository Oprator { get; }

        IElectricShopRepository ElectricShops { get; }

        ICustomerRepository Customers { get; }
        ICustomerAddressRepository CustomerAddress { get; }

        IFactorRepository Factor { get; set; }
        IAssignToFactorRepository AssignToFactor { get; }

        IForceRepository Force { get; }
        IForceRatingRepository ForceRating { get; }
        IForceCancelsRepository ForceCancels { get; }
        IForceExpertiseRepository ForceExp { get; }

        IDocumentsRepository Documents { get; }

        IStateCityRepository StateCity { get; }

        int Complete();

        Task<int> CompleteAsync();
    }
}
