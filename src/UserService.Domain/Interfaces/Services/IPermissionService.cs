using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Enums;

namespace UserService.Domain.Interfaces.Services
{
	public interface IPermissionService
	{
		Task<HashSet<PermissionEnum>> GetPermissions(Guid userId);
	}
}
