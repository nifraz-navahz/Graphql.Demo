namespace Graphql.Demo.API.Middlewares.UseUser
{
    public class AuthUserAttribute : GlobalStateAttribute
    {
        public AuthUserAttribute() : base(UserMiddleware.CONTEXT_DATA_KEY)
        {

        }
    }
}
