
namespace UserService.Domain.Options
{
	public class JwtOptions
	{
		public string Key { get; set; } = string.Empty;
        public int LifeTimeMin { get; set; }
    }
}
