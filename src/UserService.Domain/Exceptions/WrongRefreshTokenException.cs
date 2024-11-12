
namespace UserService.Domain.Exceptions
{
	public class WrongRefreshTokenException : Exception
	{
		public WrongRefreshTokenException() { }

		public WrongRefreshTokenException(string message) : base(message) { }

		public WrongRefreshTokenException(string message, Exception innerException)
		: base(message, innerException) { }
	}
}
