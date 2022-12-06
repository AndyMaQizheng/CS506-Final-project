using store_api.Core.Contexts;
using store_api.Core.Models;

namespace store_api.Core.Repositories
{
    public class AppConfigRepository : BaseRepository<AppConfig>
    {
        public AppConfigRepository(DatabaseContext context) : base(context)
        {
        }

    }
}
