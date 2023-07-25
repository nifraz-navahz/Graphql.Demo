using FirebaseAdminAuthentication.DependencyInjection.Models;
using Graphql.Demo.API.Entities;
using Graphql.Demo.API.Schema.Enums;
using Graphql.Demo.API.Schema.Subscriptions;
using Graphql.Demo.API.Services;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System;
using System.Security.Claims;

namespace Graphql.Demo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly CourseRepository _courseRepository;

        public Mutation(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [Authorize]
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            var email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            var username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            var emailVerified = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

            var newCourse = new Course
            {
                //Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };
            var course = await _courseRepository.Create(newCourse);
            var courseResult = new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId,
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courseResult);

            return courseResult;
        }

        [Authorize]
        public async Task<CourseResult> UpdateCourse(Guid courseId, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var updatedCourse = new Course
            {
                Id = courseId,
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };
            var course = await _courseRepository.Update(updatedCourse);
            var courseResult = new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId,
            };


            var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(topicName, courseResult);

            return courseResult;
        }

        [Authorize]
        public async Task<bool> DeleteCourse(Guid courseId)
        {
            try
            {
                return await _courseRepository.Delete(courseId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
