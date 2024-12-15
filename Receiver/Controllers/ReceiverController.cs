using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Receiver.Models;
using System.Threading.Tasks;

namespace Receiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiverController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReceiverController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }
    }
}
