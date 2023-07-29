namespace Graphql.Demo.API.Schema.Queries
{
    public class InstructorType : PersonType, ISearchType
    {
        public Guid Id { get; set; }
        public double Salary { get; set; }
    }
}
