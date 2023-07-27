namespace Graphql.Demo.API.Schema.Queries
{
    public class UserType
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string PhotoUrl { get; internal set; }
    }
}
