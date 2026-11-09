using Microsoft.AspNetCore.Mvc;
using ZeroFrame.Application.Interfaces;

namespace ZeroFrame.API.Controllers;

[ApiController]
[Route("api/frete")]
public class FreteController(IFreteService frete) : ControllerBase
{
    [HttpGet]
    public IActionResult Cotar([FromQuery] decimal subtotal)
    {
        if (subtotal < 0) return BadRequest("O subtotal não pode ser negativo.");
        return Ok(new { frete = frete.CalcularFrete(subtotal) });
    }
}
