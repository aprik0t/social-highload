using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialHighload.Dal.Infrastructure.Db;

namespace SocialHighload.Controllers
{
    [AllowAnonymous]
    [Route("admin/db")]
    public class DbController : Controller
    {
        private readonly DbClient _dbClient;

        public DbController(DbClient dbClient)
        {
            _dbClient = dbClient;
        }

        [HttpGet("migrate")]
        public async Task<IActionResult> MigrateDb()
        {
            await _dbClient.EnsureDbCreated();

            return Ok("Ok");
        }
    }
}