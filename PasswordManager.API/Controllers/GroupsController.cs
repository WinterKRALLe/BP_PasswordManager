using Microsoft.AspNetCore.Mvc;
using PasswordManager.API.Middleware;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GroupsController(IGroupRepository groupRepository): BaseController
{
    [HttpGet]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Group>>> GetAllGroups()
    {
        var userId = GetCurrentUserId();
        var vaults = await groupRepository.GetGroupsAsync(userId);
        return Ok(vaults);
    }
    
    [HttpPost("new")]
    [AuthorizeMiddleware]
    public async Task<ActionResult<List<Group>>> CreateGroup(AddGroupDto group)
    {
        var userId = GetCurrentUserId();
        await groupRepository.CreateGroupAndAssignRolesAsync(group.GroupName, userId, group.ShareWith);
        return Ok("Group created");
    }
}