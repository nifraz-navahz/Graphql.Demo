using Graphql.Demo.API.Schema.Enums;
using Graphql.Demo.API.Schema.Subscriptions;
using HotChocolate.Subscriptions;
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
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var newCourse = new CourseResult
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };
            _courses.Add(newCourse);

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), newCourse);

            return newCourse;
        }

        public async Task<CourseResult> UpdateCourse(Guid courseId, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var existingCourse = _courses.FirstOrDefault(x => x.Id == courseId);
            if (existingCourse == null)
            {
                throw new GraphQLException(new Error("Course does not exist!", AppErrorCodes.COURSE_NOT_FOUND.ToString()));
            }

            existingCourse.Name = courseInputType.Name;
            existingCourse.Subject = courseInputType.Subject;
            existingCourse.InstructorId = courseInputType.InstructorId;

            var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(topicName, existingCourse);

            return existingCourse;
        }

        public bool DeleteCourse(Guid courseId)
        {
            return _courses.RemoveAll(x => x.Id == courseId) > 0;
        }
    }
}
