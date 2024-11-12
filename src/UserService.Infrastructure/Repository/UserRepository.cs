﻿using Microsoft.EntityFrameworkCore;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Models;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly UserServiceDbContext _dbContext;
		public UserRepository(UserServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(User user, CancellationToken cancellationToken)
		{
			await _dbContext.Users.AddAsync(user, cancellationToken);
			await _dbContext.UserRole.AddAsync(new UserRole
			{
				UserId = user.Id,
				RoleId = (int)RoleEnum.User,
			});
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(User user, CancellationToken cancellationToken)
		{
			_dbContext.Users.Remove(user);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
		{
			return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
		}

		public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
		}

		public async Task UpdateAsync(User user, CancellationToken cancellationToken)
		{
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<HashSet<PermissionEnum>> GetPermissions(Guid userId)
		{
			var roles = await _dbContext.Users
				.AsNoTracking()
				.Include(x => x.Roles)
				.ThenInclude(x => x.Permissions)
				.Where(x => x.Id == userId)
				.Select(x => x.Roles)
				.ToListAsync();

			return roles
				.SelectMany(x => x)
				.SelectMany(x => x.Permissions)
				.Select(x => (PermissionEnum)x.Id)
				.ToHashSet();
		}
	}
}
