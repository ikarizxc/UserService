using System.Security.Cryptography;
using System.Text;
using UserService.Domain.Interfaces.Services;

namespace UserService.Domain.Services
{
	public class PasswordService : IPasswordService
	{
		public string HashPassword(string password)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				return GetHash(sha256Hash, password);
			}
		}

		public bool VerifyPassword(string password, string passwordHash)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				var hashOfInput = GetHash(sha256Hash, password);

				// Create a StringComparer an compare the hashes.
				StringComparer comparer = StringComparer.OrdinalIgnoreCase;

				return comparer.Compare(hashOfInput, passwordHash) == 0;
			}
		}

		private string GetHash(HashAlgorithm hashAlgorithm, string input)
		{

			// Convert the input string to a byte array and compute the hash.
			byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			var sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
	}
}
