using GE.Warehouse.Core.Data;
using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.Services.MobiApp
{
    public class WHMobiService : IWHMobiService
    {
        private readonly IRepository<WHMobi> _whRepo;

        public WHMobiService(IRepository<WHMobi> whRepo)
        {
            _whRepo = whRepo;
        }

        public List<WHMobi> findAll()
        {
            return _whRepo.FindAll().ToList();
        }

        public WHMobi findById(int id)
        {
            return _whRepo.FindById(id);
        }
    }
}
