using PasswordManager.Application.DTOs;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Interfaces;

public interface IGroupRepository
{
    Task<List<GroupDto>> GetGroupsAsync(int userId);
    Task<GroupDto?> GetByIdAsync(int id);
    Task CreateGroupAndAssignRolesAsync(string groupName, int userId, List<string> shareWith);
    Task<bool>  ShareToGroupAsync(int vaultToShare, List<int> groupsToShareIn);
    Task<List<Vault>> GetVaultsAsync(int groupId);
    Task<List<GroupUserRole>> GetMembersAsync(int groupId);
    Task<string> AddMember(int groupId, AddUserToGroupDto model);
}