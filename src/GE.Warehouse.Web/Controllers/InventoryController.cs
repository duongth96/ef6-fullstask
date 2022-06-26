using GE.Warehouse.Core.Infrastructure;
using GE.Warehouse.DomainObject;
using GE.Warehouse.Services.MobiApp;
using GE.Warehouse.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GE.Warehouse.Web.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IWHMobiService _whService;

        public InventoryController()
        {
            _inventoryService = EngineContext.Current.Resolve<IInventoryService>();
            _whService = EngineContext.Current.Resolve<IWHMobiService>();
        }

        [HttpPost]
        public ApiResultModel Index(InventoryTransactionModel model)
        {
            
            ApiResultModel result = new ApiResultModel();
            try
            {
                var username = ControllerContext.RequestContext.Principal.Identity.Name;
                var warehouse = _whService.findById(model.WarehouseId);
                if (warehouse == null)
                {
                    result.IsSuccessful = false;
                    result.Messages.Add("Warehouse is not found");
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json"); ;
                    throw new HttpResponseException(response);
                }

                if (model.Items == null || model.Items.Length == 0)
                {
                    result.IsSuccessful = false;
                    result.Messages.Add("Please add at least an item");
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json"); ;
                    throw new HttpResponseException(response);
                }

                var transactionId = DateTime.Now.ToString("yyyyMMddHmm") + "_" + username;

                List<IOInventory> entities = model.Items.Select((item) => {
                    IOInventory entity = new IOInventory
                    {
                        QRCode = item.QRCode,
                        Quantity = item.Quantity,
                        Status = item.Status,
                        TransactionId = transactionId,
                        Username = username,
                        WarehouseId = warehouse.Id,
                        WarehouseName = warehouse.Name,
                    };

                    return entity;
                }).ToList();
                _inventoryService.Insert(entities);
                result.IsSuccessful = true;
            } catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Messages.Add(ex.Message);
            }

            return result;
        }
    }
}
