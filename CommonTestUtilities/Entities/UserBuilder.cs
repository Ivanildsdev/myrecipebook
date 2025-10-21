using Bogus;
using CommonTestUtilities.Criptography;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build(int passwordLength = 10)
        {
            var passwordEncripter = PasswordEncripterBuilder.Build();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(u => u.Id, () => 1)
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(u => u.Password, f => passwordEncripter.Encript(password));
            
            return (user, password);
        }
    }
}
