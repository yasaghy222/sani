using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.Repositories
{
    public class StateCityRepository : GenericRepository<tblState_City>, IStateCityRepository
    {
        public StateCityRepository(SaniDBEntities context) : base(context) { }

        public List<StateCity.SelectCity> SelectCity()
        {
            return (from item in Context.tblState_City
                    where item.id != item.pid && 
                          item.tblBranch.Count() > 0
                    orderby item.title
                    select new StateCity.SelectCity
                    {
                        Text = item.title,
                        Value = item.id
                    })
                        .ToList();
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;

    }
}
