using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services
{
    public class IdentityProfileService : IProfileService
    {
        #region ...

        private readonly UserManager<UserIdentity> _userManager;
        private readonly RoleManager<UserIdentityRole> _roleManager;

        public IdentityProfileService(UserManager<UserIdentity> userManager, RoleManager<UserIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion
        #region IProfileService

        async public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await GetUser(context);
            var claims = GetClaimsFromUser(user, context.Subject.Claims);

            context.IssuedClaims = claims.ToList();
        }
        async public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp) return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }

        #endregion
        #region helpers.

        private async Task<UserIdentity> GetUser(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null) throw new ArgumentException("Invalid subject identifier");

            return user;
        }
        private IEnumerable<Claim> GetClaimsFromUser(UserIdentity user, IEnumerable<Claim> baseClaims = null)
        {
            var claims = baseClaims != null ? new List<Claim>(baseClaims) : new List<Claim>();

            //if (!string.IsNullOrWhiteSpace(user.Metadata)) claims.Add(new Claim("3S.Metadata", user.Metadata));
            //claims.Add(new Claim(JwtClaimTypes.Subject, user.Id));
            //claims.Add(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));
            //claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            //if (!string.IsNullOrWhiteSpace(user.Name)) claims.Add(new Claim("name", user.Name));
            //if (!string.IsNullOrWhiteSpace(user.LastName)) claims.Add(new Claim("last_name", user.LastName));
            //if (!string.IsNullOrWhiteSpace(user.CardNumber)) claims.Add(new Claim("card_number", user.CardNumber));
            //if (!string.IsNullOrWhiteSpace(user.CardHolderName)) claims.Add(new Claim("card_holder", user.CardHolderName));
            //if (!string.IsNullOrWhiteSpace(user.SecurityNumber)) claims.Add(new Claim("card_security_number", user.SecurityNumber));
            //if (!string.IsNullOrWhiteSpace(user.Expiration)) claims.Add(new Claim("card_expiration", user.Expiration));
            //if (!string.IsNullOrWhiteSpace(user.City)) claims.Add(new Claim("address_city", user.City));
            //if (!string.IsNullOrWhiteSpace(user.Country)) claims.Add(new Claim("address_country", user.Country));
            //if (!string.IsNullOrWhiteSpace(user.State)) claims.Add(new Claim("address_state", user.State));
            //if (!string.IsNullOrWhiteSpace(user.Street)) claims.Add(new Claim("address_street", user.Street));
            //if (!string.IsNullOrWhiteSpace(user.ZipCode)) claims.Add(new Claim("address_zip_code", user.ZipCode));
            //if (_userManager.SupportsUserEmail)
            //{
            //    claims.AddRange(new[]
            //    {
            //        new Claim(JwtClaimTypes.Email, user.Email),
            //        new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            //    });
            //}
            //if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            //{
            //    claims.AddRange(new[]
            //    {
            //        new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
            //        new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            //    });
            //}

            return claims;
        }

        #endregion
    }
}
