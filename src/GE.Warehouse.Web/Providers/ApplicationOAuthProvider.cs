using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GE.Warehouse.Core.Data;
using GE.Warehouse.Core.Infrastructure;
using GE.Warehouse.DomainObject;
using GE.Warehouse.Services.MobiApp;
using GE.Warehouse.Web.App_Start;
using GE.Warehouse.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace GE.Warehouse.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        private readonly IUserService _userService;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
            _userService = EngineContext.Current.Resolve<IUserService>();
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            // Resource owner password credentials does not provide a client ID.
            //if (context.ClientId == null)
            //{
            //    context.Validated();
            //}

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string username = context.UserName;
            string password = context.Password;
            int usermobi = -1;
            if (int.TryParse(password, out int _usermobi))
            {
                usermobi = _usermobi;
            }

            User user = _userService.ValidateUser(username, usermobi);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return Task.FromResult<object>(null);
            }

            var currentUtc = DateTime.UtcNow;
            var expiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
            var identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            //IDictionary<string, string> data = new Dictionary<string, string>
            //{
            //    { "username", user.Username }
            //};
            //AuthenticationProperties properties = new AuthenticationProperties(Create);
            AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.Username);
            properties.ExpiresUtc = expiresUtc;
            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = expiresUtc;
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(identity);


            string accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            if (user.Status == UserStatus.NotVerified)
            {
                user.Status = UserStatus.Verified;
                _userService.Update(user);
            }

            TokenModel tokenModel = new TokenModel();
            tokenModel.AccessToken = accessToken;
            tokenModel.ExpiresUtc = expiresUtc;
            return Task.FromResult<object>(tokenModel);
        }
    }
}