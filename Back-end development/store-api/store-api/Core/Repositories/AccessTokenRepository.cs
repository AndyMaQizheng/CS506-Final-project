using store_api.Core.Contexts;
using store_api.Core.Models;
using System;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class AccessTokenRepository : BaseRepository<AccessToken>
    {
        public AccessTokenRepository(DatabaseContext context) : base(context)
        {
        }

        public AccessToken FindToken(string token)
        {
            var tokenn = FindByQuery("select * from accesstokens where token = {0}", new object[] { token }).FirstOrDefault();
            return tokenn;
        }

        public AccessToken SaveToken(string token, long identifier)
        {
            var tokenn = new AccessToken
            {
                Token = token,
                CustomerId = identifier,
                CreatedBy = identifier,
                DateCreated = DateTime.UtcNow
            };

            Add(tokenn);
            return tokenn;
        }

        public void DeleteToken(long identifier)
        {
            var tokenn = FindByQuery("select * from accesstokens where identifier = {0}", new object[] { identifier }).ToList();

            foreach (var token in tokenn)
            {
                Remove(token.Id);
            }
        }


        public AccessToken RetrieveToken(long identifier)
        {
            return FindByQuery("select * from accesstokens where identifier = {0}", new object[] { identifier }).OrderByDescending(x => x.Id).FirstOrDefault();
        }


    }
}
