using Graphql.Demo.API.Models;
using System;

namespace Graphql.Demo.API.Schema.Queries
{
    public class CourseType
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Subject Subject { get; set; }
        //[GraphQLNonNullType]
        public InstructorType? Instructor { get; set; }
        public IEnumerable<StudentType>? Students { get; set; }

        public string GetDescription() => $"{Name} is the Course Name.";
    }
}
