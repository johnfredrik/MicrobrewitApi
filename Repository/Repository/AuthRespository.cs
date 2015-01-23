using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Microbrewit.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microbrewit.Repository.Interface;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Microbrewit.Repository.Repository
{
    public class AuthRepository : IDisposable, IAuthRepository
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            var provider = new DpapiDataProtectionProvider("Microbrew.it");
            _ctx = new AuthContext();
            //_userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx))
            {
                EmailService = new EmailService()
            };
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(provider.Create("ASP.NET Identity"))
            {
                //Sets the lifespan of the confirm email token and the reset password token.
                TokenLifespan = TimeSpan.FromMinutes(1),
            };
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityResult> UpdateUser(UserModel userModel)
        {
            var user = _userManager.FindByName(userModel.UserName);
            if (user == null) return new IdentityResult("User not found");

            if (!user.Email.Equals(userModel.Email)) user.Email = userModel.Email;
            return await _userManager.UpdateAsync(user);

        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }


        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public IList<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            return await _userManager.ConfirmEmailAsync(userId, code);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            return _userManager.GenerateEmailConfirmationToken(userId);
        }

        public async Task SendEmailAsync(string id, string subject, string body)
        {
            await _userManager.SendEmailAsync(id,subject,body);
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsEmailConfirmedAsync(string id)
        {
            return await _userManager.IsEmailConfirmedAsync(id);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string id)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(id);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(userId, token, password);
        }


        public async Task<IdentityUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public IdentityUser FindUserByName(string username)
        {
            return _userManager.FindByName(username);
        }

        public bool IsEmailConfirmed(string id)
        {
            return _userManager.IsEmailConfirmed(id);
        }

        public IList<string> GetUserRoles(string id)
        {
           return _userManager.GetRoles(id);
        }
    }
}
