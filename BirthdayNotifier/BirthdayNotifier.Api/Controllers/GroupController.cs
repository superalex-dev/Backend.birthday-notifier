using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayNotifier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _service;

    public GroupsController(IGroupService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groups = await _service.GetAllAsync();
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { Message = "Group created!" });
    }
}