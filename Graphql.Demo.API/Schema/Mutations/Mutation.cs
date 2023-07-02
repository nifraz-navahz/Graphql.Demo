using Graphql.Demo.API.Schema.Enums;
using System;

namespace Graphql.Demo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }
        public CourseResult CreateCourse(CourseInputType courseInputType)
        {
            var newCourse = new CourseResult
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };
            _courses.Add(newCourse);

            return newCourse;
        }

        public CourseResult UpdateCourse(Guid courseId, CourseInputType courseInputType)
        {
            var existingCourse = _courses.FirstOrDefault(x => x.Id == courseId);
            if (existingCourse == null)
            {
                throw new GraphQLException(new Error("Course does not exist!", AppErrorCodes.COURSE_NOT_FOUND.ToString()));
            }

            existingCourse.Name = courseInputType.Name;
            existingCourse.Subject = courseInputType.Subject;
            existingCourse.InstructorId = courseInputType.InstructorId;
            return existingCourse;
        }

        public bool DeleteCourse(Guid courseId)
        {
            return _courses.RemoveAll(x => x.Id == courseId) > 0;
        }
    }
}
