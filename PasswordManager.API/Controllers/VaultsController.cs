using Microsoft.AspNetCore.Mvc;
using PasswordManager.API.Middleware;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class VaultsController(IVaultRepository vaultRepository) : BaseController
{
    [HttpGet]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Vault>>> GetAllVaults()
    {
        var userId = GetCurrentUserId();
        Console.WriteLine(userId);
        var vaults = await vaultRepository.GetVaultsAsync(userId);
        return Ok(vaults);
    }

    [HttpGet("personal")]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Vault>>> GetPersonalVaults()
    {
        var userId = GetCurrentUserId();
        var vaults = await vaultRepository.GetPersonalVaultsAsync(userId);
        return Ok(vaults);
    }

    [HttpGet("trash")]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Vault>>> GetDeletedVaults()
    {
        var userId = GetCurrentUserId();
        var vaults = await vaultRepository.GetDeletedVaultsAsync(userId);
        return Ok(vaults);
    }

    [HttpPost("new")]
    [AuthorizeMiddleware]
    public async Task<IActionResult> CreateVault([FromBody] AddVaultDto addVault)
    {
        Console.WriteLine(addVault.UserName);
        var userId = GetCurrentUserId();
        var success = await vaultRepository.CreateLoginAsync(addVault, userId);
        return Ok(success);
    }
}