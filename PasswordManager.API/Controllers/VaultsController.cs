using Microsoft.AspNetCore.Mvc;
using PasswordManager.API.Middleware;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
[AuthorizeMiddleware]
public class VaultsController(IVaultRepository vaultRepository) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<VaultSummaryDto>>> GetVaultSummaries()
    {
        var userId = GetCurrentUserId();
        var vaults = await vaultRepository.GetVaultSummariesAsync(userId);
        return Ok(vaults);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vault>> GetVaultById(int id)
    {
        try
        {
            var vault = await vaultRepository.GetByIdAsync(id);
            return Ok(vault);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id:int}/trash")]
    public async Task<ActionResult<string>> MoveToTrashAsync(int id)
    {
        try
        {
            var success = await vaultRepository.MoveToTrashAsync(id);
            if (success)
            {
                return Ok(new { Message = "Vault moved to trash successfully." });
            }

            return NotFound(new { Message = "Vault not found." });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while moving the vault to trash.", Details = ex.Message });
        }
    }

    [HttpGet("personal")]
    public async Task<ActionResult<List<Vault>>> GetPersonalVaults()
    {
        var userId = GetCurrentUserId();
        var vaults = await vaultRepository.GetPersonalVaultsAsync(userId);
        return Ok(vaults);
    }

    [HttpGet("trash")]
    public async Task<ActionResult<List<Vault>>> GetDeletedVaults()
    {
        var userId = GetCurrentUserId();
        var vaults = await vaultRepository.GetDeletedVaultsAsync(userId);
        return Ok(vaults);
    }

    [HttpPost("new")]
    public async Task<IActionResult> CreateVault([FromBody] AddVaultDto addVault)
    {
        Console.WriteLine(addVault.UserName);
        var userId = GetCurrentUserId();
        var success = await vaultRepository.CreateLoginAsync(addVault, userId);
        return Ok(success);
    }
}