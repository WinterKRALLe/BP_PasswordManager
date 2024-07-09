using Microsoft.AspNetCore.Mvc;
using PasswordManager.API.Middleware;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class VaultsController(IVaultRepository vaultRepository): BaseController
{
    [HttpGet]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Vault>>> GetAllVaults()
    {
        var userId = GetCurrentUserId();
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
}