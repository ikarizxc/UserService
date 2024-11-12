using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;

namespace UserService.Domain.Services
{
	public class PermissionService : IPermissionService
	{
		private readonly IUserRepository _userRepository;

		public PermissionService(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public async Task<HashSet<PermissionEnum>> GetPermissions(Guid userId)
		{
			return await _userRepository.GetPermissions(userId);
		}
	}
}
