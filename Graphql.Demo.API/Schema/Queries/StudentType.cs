namespace Graphql.Demo.API.Schema.Queries
{
    public class StudentType : PersonType
    {
        public Guid Id { get; set; }
        [GraphQLName("gpa")]
        public double GPA { get; set; }
    }
}
