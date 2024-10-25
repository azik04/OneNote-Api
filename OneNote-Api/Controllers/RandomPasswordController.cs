using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneNote_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RandomPasswordController : ControllerBase
{
    private readonly IRandomPasswordService _service;
    public RandomPasswordController(IRandomPasswordService service)
    {
    _service = service; 
    }


    [Authorize(Policy = "User")]
    [HttpGet("RandomPassword/Lenght/16")]
    public async Task<IActionResult> RandomPassword(int lenght = 16)
    {
        var res = await _service.GeneratePasswordAsync(lenght);
        if(res.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(res);

        return BadRequest();
    }
}
