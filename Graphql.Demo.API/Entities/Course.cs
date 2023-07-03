using Graphql.Demo.API.Models;
using Graphql.Demo.API.Schema.Queries;

namespace Graphql.Demo.API.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public string GetDescription() => $"{Name} is the Course Name.";
    }
}
