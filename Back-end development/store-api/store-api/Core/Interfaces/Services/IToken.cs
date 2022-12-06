using store_api.Core.Models;

namespace store_api.Core.Interfaces.Services
{
    public interface IToken
    {
        public AccessToken GetToken(string token);
        public string GenerateTokenString(long customerid, string custemail);
        public void SaveToken(long userid, string token);
        public void DeleteToken(string identifier);
        public AccessToken RetrieveToken(long userid);

    }
}
