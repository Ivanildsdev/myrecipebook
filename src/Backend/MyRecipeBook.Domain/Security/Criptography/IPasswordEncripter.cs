namespace MyRecipeBook.Domain.Security.Criptography
{
    public interface IPasswordEncripter
    {
        public string Encript(string password);
    }
}
