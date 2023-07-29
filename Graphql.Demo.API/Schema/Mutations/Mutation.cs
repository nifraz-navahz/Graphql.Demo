using FirebaseAdminAuthentication.DependencyInjection.Models;
using Graphql.Demo.API.Entities;
using Graphql.Demo.API.Schema.Enums;
using Graphql.Demo.API.Schema.Subscriptions;
using Graphql.Demo.API.Schema.Validators;
using Graphql.Demo.API.Services;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Connections.Features;
using System;
using System.Security.Claims;

namespace Graphql.Demo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly CourseRepository _courseRepository;
        private readonly CourseInputValidator _courseInputValidator;

        public Mutation(CourseRepository courseRepository, CourseInputValidator courseInputValidator)
        {
            _courseRepository = courseRepository;
            _courseInputValidator = courseInputValidator;
        }

        //[Authorize]
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
        {
            Validate(courseInputType);

            var userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            //var email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            //var username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            //var emailVerified = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

            var newCourse = new Course
            {
                //Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
                CreatorId = userId
            };
            var course = await _courseRepository.Create(newCourse);
            var courseResult = new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId,
                //CreatorId = course.CreatorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courseResult);

            return courseResult;
        }

        private void Validate(CourseInputType courseInputType)
        {
            var result = _courseInputValidator.Validate(courseInputType);
            if (!result.IsValid)
            {
                var errorsString = result.Errors
                    .Select(x => $"Code: {x.ErrorCode}, Message: {x.ErrorMessage}")
                    .Aggregate((x1, x2) => $"{x1}; {x2}");

                throw new GraphQLException(errorsString);
            }
        }

        [Authorize]
        public async Task<CourseResult> UpdateCourse(Guid courseId, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
        {
            Validate(courseInputType);

            var userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);

            var existingCourse = await _courseRepository.GetById(courseId);

            if (existingCourse == null)
            {
                throw new GraphQLException(new Error("Course not found!", AppErrorCodes.NOT_FOUND.ToString()));
            }

            if (existingCourse.CreatorId != userId)
            {
                throw new GraphQLException(new Error("Update permission denied for the user!", AppErrorCodes.NOT_AUTHORIZED.ToString()));
            }

            existingCourse.Name = courseInputType.Name;
            existingCourse.Subject = courseInputType.Subject;
            existingCourse.InstructorId = courseInputType.InstructorId;

            var updatedCourse = await _courseRepository.Update(existingCourse);
            var courseResult = new CourseResult
            {
                Id = updatedCourse.Id,
                Name = updatedCourse.Name,
                Subject = updatedCourse.Subject,
                InstructorId = updatedCourse.InstructorId,
                //CreatorId = updatedCourse.CreatorId
            };

            var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(topicName, courseResult);

            return courseResult;
        }

        [Authorize(Policy = "isAdmin")]
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
