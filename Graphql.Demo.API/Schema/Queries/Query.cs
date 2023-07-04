using Graphql.Demo.API.Models;
using Graphql.Demo.API.Services;
using System;
namespace Graphql.Demo.API.Schema.Queries
{
    public class Query
    {
        private readonly CourseRepository _courseRepository;

        public Query(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [GraphQLDeprecated("This property is deprecated.")]
        public string Instructions => "Graphql query is working! :)";

        public async Task<IEnumerable<CourseType>> GetCoursesAsync()
        {
            var courses = await _courseRepository.GetAll();
            return courses.Select(x => new CourseType
            {
                Id = x.Id,
                Name = x.Name,
                Subject = x.Subject,
                Instructor = new InstructorType
                {
                    Id = x.Instructor.Id,
                    FirstName = x.Instructor.FirstName,
                    LastName = x.Instructor.LastName,
                    Salary = x.Instructor.Salary,
                }
            });
        }

        public async Task<CourseType?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepository.GetById(id);
            if (course == null)
            {
                return null;
            }

            return new CourseType
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                Instructor = new InstructorType
                {
                    Id = course.Instructor.Id,
                    FirstName = course.Instructor.FirstName,
                    LastName = course.Instructor.LastName,
                    Salary = course.Instructor.Salary,
                }
            };
        }
    }
}
