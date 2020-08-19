using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using IdentityModel;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Core.Framework.Infrastructure.Security.Tokens.JWT;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.AspNetCore.Http;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers
{
    public class RequestContextMapper : IModelMapper<BaseRequestContext, BaseRequestContext>
    {
        #region props.

        private const string EnglishCulture = "en-US";
        private const string ArabicCulture = "ar-EG";
        private const string DefaultCulture = "en-US";
        
        private const string OrganizationIdKey = "organization-id";
        private const string OrganizationAdminKey = "organization-admin";

        private readonly IHttpContextAccessor _httpContextAccessor;
        public static RequestContextMapper Instance = new RequestContextMapper();

        #endregion
        #region cst.

        public RequestContextMapper()
        {
        }
        public RequestContextMapper(IHttpContextAccessor httpContextAccessor) : this()
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion
        #region IModelMapper

        public BaseRequestContext Map(BaseRequestContext from, object metadata = null)
        {
            if (from == null) return null;

            from.Header = from.Header ?? new BaseRequestContext.HeaderContent();

            var jwt = this._httpContextAccessor != null 
                    ? JWTUtilities.ExtractJwtToken(this._httpContextAccessor.HttpContext.Request)
                    : JWTUtilities.ExtractJwtToken(from?.Header?.JWT);

            from.Header.CorrelationId = GetCorrelationId(from.Header.CorrelationId);
            from.Header.SessionId = GetSessionId(from.Header.SessionId, jwt);
            from.Header.UserRoles = GetUserRoles(from.Header.UserRoles, jwt);
            from.Header.UserId = GetUserId(from.Header.UserId, jwt);
            from.Header.UserLoginId = GetUserLoginId(from.Header.UserLoginId, jwt);
            from.Header.UserOrganizationId = GetUserOrganizationId(from.Header.UserOrganizationId, jwt);
            from.Header.IsOrganizationAdmin = GetUserOrganizationAdminStatus(from.Header.IsOrganizationAdmin, jwt);
            from.Header.RequestTimeUTC = DateTime.UtcNow;
            from.Header.JWT = GetGWTNative(from.Header.JWT, jwt);

            from.Header.Culture = GetCulture(from.Header.Culture);
            ApplyCulture(from.Header.Culture);

            return from;
        }
        public TDestinationAlt Map<TDestinationAlt>(BaseRequestContext from, object metadata = null) where TDestinationAlt : BaseRequestContext
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
        #region helpers.

        private string GetCorrelationId(string placeHolder)
        {
            placeHolder = !string.IsNullOrWhiteSpace(placeHolder)
                        ? placeHolder
                        : this._httpContextAccessor?.HttpContext?.TraceIdentifier
                        ?? Guid.NewGuid().ToString();

            return placeHolder;
        }
        private string GetSessionId(string placeHolder, JwtSecurityToken jwt = null)
        {
            return !string.IsNullOrWhiteSpace(placeHolder)
                 ? placeHolder
                 : GetClaimValue(JwtClaimTypes.SessionId, jwt);
        }
        private string GetUserId(string placeHolder, JwtSecurityToken jwt = null)
        {
            return !string.IsNullOrWhiteSpace(placeHolder)
                 ? placeHolder
                 : this._httpContextAccessor?.HttpContext?.User?.Identity?.Name
                ?? GetClaimValue(JwtClaimTypes.Name, jwt);
        }
        private string GetUserLoginId(string placeHolder, JwtSecurityToken jwt = null)
        {
            return !string.IsNullOrWhiteSpace(placeHolder)
                 ? placeHolder
                 : GetClaimValue(JwtClaimTypes.Email, jwt);
        }
        private IEnumerable<string> GetUserRoles(IEnumerable<string> placeHolder, JwtSecurityToken jwt = null)
        {
            return placeHolder != null && placeHolder.Count() > 0
                 ? placeHolder
                 : GetClaimValues(JwtClaimTypes.Role, jwt);
        }
        private string GetUserOrganizationId(string placeHolder, JwtSecurityToken jwt = null)
        {
            return !string.IsNullOrWhiteSpace(placeHolder)
                 ? placeHolder
                 : GetClaimValue(RequestContextMapper.OrganizationIdKey, jwt);
        }
        private bool? GetUserOrganizationAdminStatus(bool? placeHolder, JwtSecurityToken jwt = null)
        {
            return placeHolder.HasValue
                 ? placeHolder
                 : MapBool(GetClaimValue(RequestContextMapper.OrganizationAdminKey, jwt));
        }
        private string GetGWTNative(string placeHolder, JwtSecurityToken jwt = null)
        {
            return !string.IsNullOrWhiteSpace(placeHolder)
                 ? placeHolder
                 : JWTUtilities.ExtractJwtEncodedToken(this._httpContextAccessor.HttpContext.Request)
                 ?? JWTUtilities.ExtractJwtEncodedToken(jwt);
        }
        private string GetCulture(string placeHolder)
        {
            placeHolder = !string.IsNullOrWhiteSpace(placeHolder)
                        ? placeHolder
                        : GetHeaderValue("Accept-Language")
                        ?? GetQueryStringValue("language")
                        ?? RequestContextMapper.DefaultCulture;

            return placeHolder.Contains("ar")
                 ? RequestContextMapper.ArabicCulture
                 : RequestContextMapper.EnglishCulture;
        }
        private void ApplyCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture)) return;
            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower() == culture.ToLower()) return;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        private string GetHeaderValue(string key)
        {
            try
            {
                return this._httpContextAccessor?.HttpContext?.Request?.Headers[key].FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        private string GetQueryStringValue(string key)
        {
            return this._httpContextAccessor?.HttpContext?.Request?.Query[key].ToString();
        }
        private string GetClaimValue(string key, JwtSecurityToken jwt = null)
        {
            var claims = GetEffectiveClaims(this._httpContextAccessor?.HttpContext, jwt);
            return claims?.Where(x => x.Type == key)?.FirstOrDefault()?.Value;
        }
        private IEnumerable<string> GetClaimValues(string key, JwtSecurityToken jwt = null)
        {
            var claims = GetEffectiveClaims(this._httpContextAccessor?.HttpContext, jwt);
            return claims?.Where(x => x.Type == key)?.Select(x=>x.Value).ToList();
        }
        private IEnumerable<Claim> GetEffectiveClaims(HttpContext httpContext, JwtSecurityToken jwt)
        {
            var nativeClaims = httpContext?.User?.Claims;
            var extractClaims = jwt?.Claims;

            return (nativeClaims != null && nativeClaims.Count() > 0)
                 ? nativeClaims
                 : extractClaims;
        }

        private bool? MapBool(string from)
        {
            return string.IsNullOrWhiteSpace(from)
                ? null
                : bool.TryParse(from, out bool to)
                ? (bool?) to
                : null;
        }

        #endregion
    }
}
