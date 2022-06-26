using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
using Chatbox.Core;
using Chatbox.Core.Configuration;
using Chatbox.Core.Infrastructure;
using UserAgentStringLibrary;

namespace Chatbox.Services.Helpers
{
    /// <summary>
    /// User agent helper
    /// </summary>
    public class UserAgentHelper : IUserAgentHelper
    {
        private readonly MvcCoreConfig _config;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="httpContext">HTTP context</param>
        public UserAgentHelper(MvcCoreConfig config, IWebHelper webHelper, HttpContextBase httpContext)
        {
            _config = config;
            _webHelper = webHelper;
            _httpContext = httpContext;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual UasParser GetUasParser()
        {
            if (Singleton<UasParser>.Instance == null)
            {
                //no database created
                if (String.IsNullOrEmpty(_config.UserAgentStringsPath))
                    //UNDONE: maybe, throw an exception?
                    return null;

                var filePath = _webHelper.MapPath(_config.UserAgentStringsPath);
                var uasParser = new UasParser(filePath);
                Singleton<UasParser>.Instance = uasParser;
            }
            return Singleton<UasParser>.Instance;
        }

        /// <summary>
        /// Get a value indicating whether the request is made by search engine (web crawler)
        /// </summary>
        /// <returns>Result</returns>
        public virtual bool IsSearchEngine()
        {
            if (_httpContext == null)
                return false;

            //we put required logic in try-catch block
            //more info: http://www.nopcommerce.com/boards/t/17711/unhandled-exception-request-is-not-available-in-this-context.aspx
            bool result = false;
            try
            {
                var uasParser = GetUasParser();
                var userAgent = _httpContext.Request.UserAgent;
                result = uasParser.IsBot(userAgent);
                //result = context.Request.Browser.Crawler;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }
            return result;
        }

    }
}