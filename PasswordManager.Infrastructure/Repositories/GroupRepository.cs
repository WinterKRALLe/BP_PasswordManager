using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Domain.Enums;
using PasswordManager.Infrastructure.Database.Context;

namespace PasswordManager.Infrastructure.Repositories;

public class GroupRepository(ApplicationDbContext dbContextFactory, IUserRepository userRepository) : IGroupRepository
{
    protected readonly ApplicationDbContext Context = dbContextFactory;

    public async Task<List<Group>> GetGroupsAsync(int userId)
    {
        return await Context.GroupUserRoles
            .Where(gur => gur.UserId == userId)
            .Include(gur => gur.Group)
            .ThenInclude(g => g.Members)
            .ThenInclude(m => m.User)
            .Select(gur => gur.Group)
            .ToListAsync();
    }

    public async Task CreateGroupAndAssignRolesAsync(string groupName, int userId, List<string> shareWith)
    {
        var group = new Group
        {
            Title = groupName
        };
        Context.Groups.Add(group);
        await Context.SaveChangesAsync();

        var ownerRole = new GroupUserRole
        {
            UserId = userId,
            Role = GroupRoles.Owner,
            Group = group
        };
        Context.GroupUserRoles.Add(ownerRole);

        foreach (var username in shareWith)
        {
            var member = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            var memberRole = new GroupUserRole
            {
                UserId = member!.Id,
                Role = GroupRoles.Member,
                Group = group
            };
            Context.GroupUserRoles.Add(memberRole);
        }

        await Context.SaveChangesAsync();
    }

    public async Task<bool> ShareToGroupAsync(Vault vaultToShare, List<GroupDto> groupsToShareIn)
    {
        foreach (var groupModel in groupsToShareIn)
        {
            var group = await Context.Groups.FindAsync(groupModel.Id);
            if (group == null) continue;
            if (group.GroupVaults.All(gv => gv.VaultId != vaultToShare.Id))
            {
                group.GroupVaults.Add(new GroupVault { VaultId = vaultToShare.Id });
            }
        }

        return await Context.SaveChangesAsync() > 0;
    }

    public async Task<List<Vault>> GetVaultsAsync(int groupId)
    {
        return await Context.GroupVaults
            .Where(gv => gv.GroupId == groupId)
            .Include(gv => gv.Vault)
            .Select(gv => gv.Vault)
            .ToListAsync();
    }

    public async Task<string> AddMember(int groupId, AddUserToGroupDto model)
    {
        var memberToAdd = await userRepository.GetByUsernameAsync(model.UserName);
        if (memberToAdd == null)
        {
            return "User not found.";
        }

        var existingMembership = await Context.GroupUserRoles
            .SingleOrDefaultAsync(gur => gur.UserId == memberToAdd.Id && gur.GroupId == groupId);

        if (existingMembership != null)
        {
            return "User is already a member.";
        }

        var newGroupUserRole = new GroupUserRole
        {
            UserId = memberToAdd.Id,
            GroupId = groupId,
            Role = GroupRoles.Member,
            User = memberToAdd
        };

        Context.GroupUserRoles.Add(newGroupUserRole);

        await Context.SaveChangesAsync();

        return "User has been added to the group.";
    }
}