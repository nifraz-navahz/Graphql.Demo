namespace Graphql.Demo.API.Entities
{
    public class Instructor: Person
    {
        public Guid Id { get; set; }
        public double Salary { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
