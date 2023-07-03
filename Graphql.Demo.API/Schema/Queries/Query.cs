using Bogus;
using Graphql.Demo.API.Models;
using System;
namespace Graphql.Demo.API.Schema.Queries
{
    public class Query
    {
        private readonly Faker<StudentType> _studentTypeFaker;
        private readonly Faker<InstructorType> _instructorTypeFaker;
        private readonly Faker<CourseType> _courseTypeFaker;

        public Query()
        {
            _studentTypeFaker = new Faker<StudentType>()
                .RuleFor(x => x.Id, y => Guid.NewGuid())
                .RuleFor(x => x.FirstName, y => y.Name.FirstName())
                .RuleFor(x => x.LastName, y => y.Name.LastName())
                .RuleFor(x => x.GPA, y => y.Random.Double(1, 4));

            _instructorTypeFaker = new Faker<InstructorType>()
                .RuleFor(x => x.Id, y => Guid.NewGuid())
                .RuleFor(x => x.FirstName, y => y.Name.FirstName())
                .RuleFor(x => x.LastName, y => y.Name.LastName())
                .RuleFor(x => x.Salary, y => y.Random.Double(10000, 20000));

            _courseTypeFaker = new Faker<CourseType>()
                .RuleFor(x => x.Id, y => Guid.NewGuid())
                .RuleFor(x => x.Name, y => y.Name.JobTitle())
                .RuleFor(x => x.Subject, y => y.PickRandom<Subject>())
                .RuleFor(x => x.Instructor, y => _instructorTypeFaker.Generate())
                .RuleFor(x => x.Students, y => _studentTypeFaker.Generate(3));
        }

        [GraphQLDeprecated("This property is deprecated.")]
        public string Instructions => "Graphql query is working! :)";

        public IEnumerable<CourseType> GetCourses()
        {
            var list = _courseTypeFaker.Generate(5);

            return list;
        }

        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            await Task.Delay(3000);
            var course = _courseTypeFaker.Generate();
            course.Id = id;
            return course;
        }
    }
}
