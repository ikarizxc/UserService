
namespace UserService.Domain.Exceptions
{
	public class CredentialsInUseException : Exception
	{
		public CredentialsInUseException() { }

		public CredentialsInUseException(string message) : base(message) { }

		public CredentialsInUseException(string message, Exception innerException)
		: base(message, innerException) { }
	}
}
