namespace Graphql.Demo.API.Entities
{
    public class Student: Person
    {
        public Guid Id { get; set; }
        public double GPA { get; set; }
    }
}
