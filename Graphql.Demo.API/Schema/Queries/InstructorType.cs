namespace Graphql.Demo.API.Schema.Queries
{
    public class InstructorType : PersonType
    {
        public Guid Id { get; set; }
        public double Salary { get; set; }
    }
}
