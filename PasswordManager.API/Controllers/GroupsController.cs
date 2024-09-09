using Microsoft.AspNetCore.Mvc;
using PasswordManager.API.Middleware;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthorizeMiddleware]
public class GroupsController(IGroupRepository groupRepository) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<Group>>> GetAllGroups()
    {
        var userId = GetCurrentUserId();
        var vaults = await groupRepository.GetGroupsAsync(userId);
        return Ok(vaults);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroupById(int id)
    {
        var group = await groupRepository.GetByIdAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost("new")]
    public async Task<ActionResult<List<Group>>> CreateGroup([FromBody] AddGroupDto group)
    {
        var userId = GetCurrentUserId();
        group.ShareWith ??= [];

        await groupRepository.CreateGroupAndAssignRolesAsync(group.GroupName, userId, group.ShareWith);
        return Ok("Group created");
    }

    [HttpPost("share")]
    public async Task<ActionResult> ShareToGroup([FromBody] ShareWithGroupDto shareVaultDto)
    {
        var success = await groupRepository.ShareToGroupAsync(shareVaultDto.VaultId, shareVaultDto.GroupsId);
        if (success)
        {
            return Ok("Vault shared to groups successfully.");
        }

        return BadRequest("Failed to share vault to groups.");
    }

    [HttpGet("{groupId}/vaults")]
    public async Task<ActionResult<List<Vault>>> GetVaultsByGroupId(int groupId)
    {
        var vaults = await groupRepository.GetVaultsAsync(groupId);
        return Ok(vaults);
    }

    [HttpGet("{groupId}/members")]
    public async Task<ActionResult<List<GroupUserRole>>> GetMembersByGroupId(int groupId)
    {
        var members = await groupRepository.GetMembersAsync(groupId);
        return Ok(members);
    }
    
    [HttpPost("{groupId}/members")]
    public async Task<ActionResult<string>> AddMemberToGroup(int groupId, AddUserToGroupDto model)
    {
        var result = await groupRepository.AddMember(groupId, model);
        return result switch
        {
            "User not found." => NotFound(result),
            "User is already a member." => BadRequest(result),
            _ => Ok(result)
        };
    }
}