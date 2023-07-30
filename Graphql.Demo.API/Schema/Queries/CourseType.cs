using FirebaseAdmin.Auth;
using Graphql.Demo.API.Models;
using Graphql.Demo.API.Services;
using Graphql.Demo.API.Services.DataLoaders;
using System;

namespace Graphql.Demo.API.Schema.Queries
{
    public class CourseType : ISearchType
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Subject Subject { get; set; }
        //[GraphQLIgnore]
        [IsProjected(true)]
        public Guid InstructorId { get; set; }
        //[GraphQLNonNullType]
        public async Task<InstructorType?> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            var instructor = await instructorDataLoader.LoadAsync(InstructorId);
            if (instructor == null) { return null; }

            return new InstructorType
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary,
            };
        }
        public IEnumerable<StudentType>? Students { get; set; }
        [IsProjected(true)]
        public string? CreatorId { get; set; }
        public async Task<UserType?> Creator([Service] UserDataLoader userDataLoader)
        {
            if (string.IsNullOrWhiteSpace(CreatorId)) { return null; }
            return await userDataLoader.LoadAsync(CreatorId);
        }
        public string GetDescription() => $"{Name} is the Course Name.";
    }
}
