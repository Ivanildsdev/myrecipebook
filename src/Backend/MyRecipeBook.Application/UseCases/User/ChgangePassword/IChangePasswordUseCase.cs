using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChgangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
