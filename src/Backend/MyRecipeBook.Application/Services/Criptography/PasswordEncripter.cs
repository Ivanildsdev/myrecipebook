using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Criptography
{
    public class PasswordEncripter
    {
        private readonly string _aditionalKey;
        public PasswordEncripter(string aditionalKey)
        {
            _aditionalKey = aditionalKey;
        }

        public string Encript(string password)
        {
            var newPassword = password + _aditionalKey;
            var bytes = Encoding.UTF8.GetBytes(newPassword);
            var hashBytes = SHA512.HashData(bytes);

            return StringFromByteArray(hashBytes);

        }

        private string StringFromByteArray(byte[] hashBytes)
        {
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            { 
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
