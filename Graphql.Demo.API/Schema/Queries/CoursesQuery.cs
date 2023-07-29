using Graphql.Demo.API.Schema.Filters;
using Graphql.Demo.API.Schema.Sorters;
using Graphql.Demo.API.Services;
using Microsoft.EntityFrameworkCore;

namespace Graphql.Demo.API.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class CoursesQuery
    {
        private readonly CourseRepository _courseRepository;

        public CoursesQuery(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        //public async Task<IEnumerable<CourseType>> GetCoursesAsync()
        //{
        //    var courses = await _courseRepository.GetAll();
        //    return courses.Select(x => new CourseType
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        Subject = x.Subject,
        //        InstructorId = x.InstructorId,
        //    });
        //}

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
                CreatorId = x.CreatorId
            });
        }

        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting(typeof(CourseSortType))]
        public IQueryable<CourseType> GetCourses([Service(ServiceKind.Synchronized)] SchoolDbContext context)
        {
            return context.Courses
                .Select(x => new CourseType
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subject = x.Subject,
                    InstructorId = x.InstructorId,
                    CreatorId = x.CreatorId
                }
                );
        }

        public async Task<IEnumerable<ISearchType>> SearchAsync([Service(ServiceKind.Synchronized)] SchoolDbContext context, string term)
        {
            var courses = await context.Courses
                .Where(x => x.Name.ToLower().Contains(term.ToLower()))
                .Select(x => new CourseType
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subject = x.Subject,
                    InstructorId = x.InstructorId,
                    CreatorId = x.CreatorId
                }
                )
                .ToListAsync();
            var instructors = await context.Instructors
                .Where(x => x.FirstName.ToLower().Contains(term.ToLower()) || x.LastName.ToLower().Contains(term.ToLower()))
                .Select(x => new InstructorType
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Salary = x.Salary
                }
                )
                .ToListAsync();

            return new List<ISearchType>()
                .Concat(courses)
            .Concat(instructors);
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
                CreatorId = course.CreatorId
            };
        }
    }
}
