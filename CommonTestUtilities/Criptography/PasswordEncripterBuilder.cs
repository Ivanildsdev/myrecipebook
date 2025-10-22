using MyRecipeBook.Infrastructure.Security.Criptography;

namespace CommonTestUtilities.Criptography
{
    public class PasswordEncripterBuilder
    {
        public static Sha512Encripter Build() => new Sha512Encripter("abc123");
    }
}
