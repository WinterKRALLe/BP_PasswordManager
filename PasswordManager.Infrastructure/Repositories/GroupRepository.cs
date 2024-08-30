using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.DTOs;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Domain.Enums;
using PasswordManager.Infrastructure.Database.Context;

namespace PasswordManager.Infrastructure.Repositories;

public class GroupRepository(ApplicationDbContext dbContextFactory,
    IUserRepository userRepository, IKeyService keyService, IAesService aesService) : IGroupRepository
{
    protected readonly ApplicationDbContext Context = dbContextFactory;

    public async Task<List<GroupDto>> GetGroupsAsync(int userId)
    {
        return await Context.Groups
            .Where(g => g.Members.Any(m => m.UserId == userId))
            .Select(g => new GroupDto
            {
                Id = g.Id,
                Title = g.Title
            })
            .ToListAsync();
    }

    public Task<GroupDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
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

        if (shareWith.Count != 0)
        {
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
        }

        await Context.SaveChangesAsync();
    }

    public async Task<bool> ShareToGroupAsync(int vaultToShare, List<int> groupsToShareIn)
    {
        foreach (var groupId in groupsToShareIn)
        {
            var group = await Context.Groups.FindAsync(groupId);
            if (group == null) continue;
            if (group.GroupVaults.All(gv => gv.VaultId != vaultToShare))
            {
                group.GroupVaults.Add(new GroupVault { VaultId = vaultToShare });
            }
        }

        return await Context.SaveChangesAsync() > 0;
    }

    public async Task<List<Vault>> GetVaultsAsync(int groupId)
    {
        var vaults = await Context.GroupVaults.Where(gv => gv.GroupId == groupId)
            .Include(gv => gv.Vault)
            .Select(gv => gv.Vault)
            .ToListAsync();

        if (vaults.Count == 0) return [];

        foreach (var vault in vaults)
        {
            var encryptedKey = vault.EncryptedKey;
            var decryptedKey = await keyService.DecryptKey(encryptedKey);
            var decryptedPassword = await aesService.DecryptAsync(vault.Password, decryptedKey);
            vault.Password = decryptedPassword;
        }

        return vaults;
    }

    public async Task<List<GroupUserRole>> GetMembersAsync(int groupId)
    {
        var members = await Context.GroupUserRoles.Where(gv => gv.GroupId == groupId)
            .Include(gur => gur.User)
            .ToListAsync();

        return members;
    }

    public async Task<string> AddMember(int groupId, AddUserToGroupDto model)
    {
        var memberToAdd = await userRepository.GetByUsernameAsync(model.Email);
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


