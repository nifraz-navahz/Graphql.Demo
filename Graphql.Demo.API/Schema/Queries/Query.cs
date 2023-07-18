using Graphql.Demo.API.Models;
using Graphql.Demo.API.Schema.Filters;
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
                InstructorId = x.InstructorId,
            });
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        public async Task<IEnumerable<CourseType>> GetCoursesCursorPagedAsync()
        {
            var courses = await _courseRepository.GetAll();
            return courses.Select(x => new CourseType
            {
                Id = x.Id,
                Name = x.Name,
                Subject = x.Subject,
                InstructorId = x.InstructorId,
            });
        }

        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseFiltering(typeof(CourseFilterType))]
        public IQueryable<CourseType> GetCoursesOffsetPaged([Service(ServiceKind.Synchronized)] SchoolDbContext context)
        {
            return context.Courses
                .OrderBy(x => x.Id)
                .Select(x => new CourseType
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Subject = x.Subject,
                        InstructorId = x.InstructorId,
                    }
                );
        }

        public async Task<CourseType?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepository.GetById(id);
            if (course == null) { return null; }

            return new CourseType
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId,
            };
        }
    }
}
