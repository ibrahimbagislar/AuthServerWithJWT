using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userName = User.Identity?.Name;
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //veritabanından userId veya userName üzerinden gerekli verileri çekeceğiz
            return Ok($"Fatura İşlemleri => serName : {userName} - UserId : {userId}");
        }
    }
}
