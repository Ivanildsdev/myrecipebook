using MyRecipeBook.Application.Services.Criptography;

namespace CommonTestUtilities.Criptography
{
    public class PasswordEncripterBuilder
    {
        public static PasswordEncripter Build() => new PasswordEncripter("abc123");
    }
}
