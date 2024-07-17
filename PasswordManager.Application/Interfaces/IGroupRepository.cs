using PasswordManager.Application.DTOs;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Interfaces;

public interface IGroupRepository
{
    Task<List<Group>> GetGroupsAsync(int userId);
    Task CreateGroupAndAssignRolesAsync(string groupName, int userId, List<string> shareWith);
    Task<bool> ShareToGroupAsync(Vault vaultToShare, List<GroupDto> groupsToShareIn);
    Task<List<Vault>> GetVaultsAsync(int groupId);
    Task<string> AddMember(int groupId, AddUserToGroupDto model);
}