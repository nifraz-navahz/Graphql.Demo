using FirebaseAdminAuthentication.DependencyInjection.Models;
using Graphql.Demo.API.Models;
using HotChocolate.Resolvers;
using SQLitePCL;
using System;
using System.Security.Claims;

namespace Graphql.Demo.API.Middlewares.UseUser
{
    public class UserMiddleware
    {
        public const string CONTEXT_DATA_KEY = "User";
        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            if (context.ContextData.TryGetValue("ClaimsPrincipal", out object? rawClaimsPrincipal) && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {
                bool emailVerified = bool.TryParse(claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED), out bool result) && result;
                var user = new User
                {
                    Id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID),
                    Email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL),
                    Username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME),
                    EmailVerified = emailVerified,
                };
                context.ContextData.Add(CONTEXT_DATA_KEY, user);
            }
            await _next.Invoke(context);
        }
    }
}
