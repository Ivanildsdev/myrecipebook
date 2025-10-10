using Moq;
using MyRecipeBook.Infrastructure.DataAccess;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            var moq = new Mock<IUnitOfWork>();

            return moq.Object;
        }
    }
}
