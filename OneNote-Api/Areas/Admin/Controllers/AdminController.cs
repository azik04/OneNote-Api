using BLL.Services.Interfaces;
using Domain.DTO_s.Users;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneNote_Api.Areas.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
[Area("Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _service;
    public AdminController(IUserService service)
    {
        _service = service;
    }


    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDTO vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var data = await _service.Create(vm);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAll();
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var data = await _service.GetById(id);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var data = await _service.Delete(id);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "Admin")]
    [HttpPut("{id}/ChangePassword")]
    public async Task<IActionResult> ChangePassword(long id, ChangePasswordDTO vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var data = await _service.ChangePassword(id, vm);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy ="Admin")]
    [HttpPut("{id}/ChangeRole")]
    public async Task<IActionResult> ChangeRole(long id, Role role)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var data = await _service.ChangeRole(id, role);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }
}
