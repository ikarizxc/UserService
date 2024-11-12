namespace UserService.Domain.Models
{
	public class User
	{
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
        public ICollection<Role> Roles { get; set; } = [];
    }
}
