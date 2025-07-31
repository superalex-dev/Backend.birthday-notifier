using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces;
using BirthdayNotifier.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayNotifier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BirthdayEntriesController : ControllerBase
{
    private readonly IBirthdayService _birthdayService;

    public BirthdayEntriesController(IBirthdayService birthdayService)
    {
        _birthdayService = birthdayService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BirthdayEntryDto dto)
    {
        await _birthdayService.AddAsync(dto);
        return Ok(new { Message = "Birthday entry created." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entries = await _birthdayService.GetAllAsync();
        return Ok(entries);
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysBirthdays()
    {
        var entries = await _birthdayService.GetTodaysBirthdaysAsync();
        return Ok(entries);
    }
}