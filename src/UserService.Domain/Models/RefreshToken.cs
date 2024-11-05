namespace UserService.Domain.Models
{
	public class RefreshToken
	{
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid UserId { get; set; }
    }
}
