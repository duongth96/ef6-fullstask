using GE.Warehouse.Core.Infrastructure;
using GE.Warehouse.DomainObject;
using GE.Warehouse.Services.MobiApp;
using GE.Warehouse.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GE.Warehouse.Web.Controllers
{

    [Authorize]
    public class WarehouseController : ApiController
    {
        private readonly IWHMobiService _whService;
        public WarehouseController()
        {
            _whService = EngineContext.Current.Resolve<IWHMobiService>();
        }

        [HttpGet]
        public ApiResultModel<List<WHMobi>> Index()
        {
            var result = new ApiResultModel<List<WHMobi>>();

            try
            {
                result.Data = _whService.findAll();
                result.IsSuccessful = true;
            } catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.Messages.Add(ex.Message);
            }
            return result;
        }
    }
}
