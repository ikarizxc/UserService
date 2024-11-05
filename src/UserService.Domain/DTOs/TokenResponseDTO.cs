namespace UserService.Domain.DTOs
{
	public class TokenResponseDTO
	{
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
