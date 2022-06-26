using System.Collections.Generic;
using System.Web.Mvc;

namespace GE.Warehouse.Web.Framework.Mvc
{
    /// <summary>
    /// Base nopCommerce model
    /// </summary>
    public partial class BaseMvcModel
    {
        public BaseMvcModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
        }
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Use this property to store any custom value for your models. 
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// Base nopCommerce entity model
    /// </summary>
    public partial class BaseMvcEntityModel : BaseMvcModel
    {
        public int Id { get; set; }

    }
}
