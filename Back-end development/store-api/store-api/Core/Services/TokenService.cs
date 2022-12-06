using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using store_api.Core.Interfaces;
using store_api.Core.Interfaces.Services;
using store_api.Core.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace store_api.Core.Services
{
    public class TokenService : BaseService, IToken
    {
        private ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger, IUnitOfWork work) : base(work)
        {
            _logger = logger;
        }

        public AccessToken GetToken(string token)
        {
            return _work.AccessTokenRepository.FindToken(token);
        }

        public string GenerateTokenString(long customerid, string custemail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Get("jwt_secret"));
            var created = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(Get("jwt_expiry_minutes")));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, customerid.ToString()),
                    new Claim(ClaimTypes.Email , custemail)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = created
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void SaveToken(long customerid, string token)
        {
            _work.AccessTokenRepository.SaveToken(token, customerid);
            _work.Commit();
        }
        public void DeleteToken(string identifier)
        {

            var customer = _work.CustomerRepository.Find(Convert.ToInt64(identifier));
            _work.AccessTokenRepository.DeleteToken(customer.Id);
            _work.Commit();
        }

        public AccessToken RetrieveToken(long userid)
        {
            return _work.AccessTokenRepository.RetrieveToken(userid);
        }
    }
}
