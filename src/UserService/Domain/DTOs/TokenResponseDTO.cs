namespace UserService.Domain.DTOs
{
	public class TokenResponseDTO
	{
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Type { get; set; }
    }
}
