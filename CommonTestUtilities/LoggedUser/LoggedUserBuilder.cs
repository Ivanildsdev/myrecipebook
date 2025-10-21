﻿using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace CommonTestUtilities.LoggedBuilder
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();
            mock.Setup(lu => lu.User()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
