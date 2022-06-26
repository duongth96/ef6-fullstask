using GE.Warehouse.Core.Data;
using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.Services.MobiApp
{
    public class IOInventoryService : IInventoryService
    {
        private readonly IRepository<IOInventory> _repo;

        public IOInventoryService(IRepository<IOInventory> repo)
        {
            _repo = repo;
        }

        public List<IOInventory> Insert(List<IOInventory> entities)
        {
            _repo.Insert(entities);

            return entities;
        }
    }
}
