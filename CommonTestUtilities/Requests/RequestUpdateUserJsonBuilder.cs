using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateUserJsonBuilder
    {
        public static RequestUpdateUserJson Build(int passwordLength = 10)
        {
            return new Faker<RequestUpdateUserJson>()
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Name));
        }
    }
}
