using System;
using System.Web;
using System.Web.Mvc;

namespace GE.Warehouse.Web.Framework
{
    public abstract class ExtendedAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            //Invariant.IsNotNull(filterContext, "filterContext");

            if (filterContext.IsChildAction)
            {
                return;
            }

            if (IsAuthorized(filterContext))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;

                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, filterContext);
            }
            else
            {
                HandleUnauthorized(filterContext);
            }
        }

        /// <summary>
        /// Determines whether the specified filter context is authorized.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>
        /// <c>true</c> if the specified filter context is authorized; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsAuthorized(AuthorizationContext filterContext);

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected virtual void HandleUnauthorized(AuthorizationContext filterContext)
        {
            //Invariant.IsNotNull(filterContext, "filterContext");

            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Called when cache module verifies the cache status.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns></returns>
        protected virtual HttpValidationStatus OnCacheAuthorization(AuthorizationContext filterContext)
        {
            //Invariant.IsNotNull(filterContext, "filterContext");

            bool authorized = IsAuthorized(filterContext);

            return authorized ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization((AuthorizationContext)data);
        }
    }
}
