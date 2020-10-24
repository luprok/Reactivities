using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<User> { }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IJwtGenerator jwtGenerator;
            private readonly IUserAccessor userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.jwtGenerator = jwtGenerator;
                this.userManager = userManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                // handler logic goes here
                var user = await userManager.FindByNameAsync(userAccessor.GetCurrentUsername());

                return new User 
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Token=jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
        }
    }
}