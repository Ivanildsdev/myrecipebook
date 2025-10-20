using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build(int passwordLength = 10)
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Email))
                .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength)).Generate();
        }
    }
}
