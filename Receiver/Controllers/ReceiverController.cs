using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Receiver.Models;
using System.Linq;
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
            if (!_context.Account.Any())
            {
                _context.Account.Add(new Account() {  Balance = 100 });
                _context.Account.Add(new Account() {  Balance = 100 });
                _context.SaveChanges();
            }
      
            return Ok(await _context.Account.ToListAsync());
        }
    }
}
