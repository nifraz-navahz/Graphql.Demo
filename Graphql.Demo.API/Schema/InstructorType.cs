namespace Graphql.Demo.API.Schema
{
    public class InstructorType: PersonType
    {
        public Guid Id { get; set; }
        public double Salary { get; set; }
    }
}
