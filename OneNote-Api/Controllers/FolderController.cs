using BLL.Services.Interfaces;
using Domain.DTO_s.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneNote_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FolderController : ControllerBase
{
    private readonly IFolderService _service;
    public FolderController(IFolderService service)
    {
        _service = service;
    }


    [Authorize(Policy = "User")]
    [HttpPost]
    public async Task<IActionResult> Create(FolderDTO vm)
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


    [Authorize(Policy = "User")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAll();
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "User")]
    [HttpGet("Section")]
    public async Task<IActionResult> GetBySection(long folderId)
    {
        var data = await _service.GetBySection(folderId);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var data = await _service.GetById(id);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var data = await _service.Remove(id);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }


    [Authorize(Policy = "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, FolderDTO vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var data = await _service.Update(id, vm);
        if (data.StatusCode == Domain.Enum.StatusCode.OK)
            return Ok(data);

        return BadRequest(data);
    }
}
