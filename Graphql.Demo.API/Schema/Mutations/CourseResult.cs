﻿using Graphql.Demo.API.Models;
using Graphql.Demo.API.Schema.Queries;

namespace Graphql.Demo.API.Schema.Mutations
{
    public class CourseResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
        public string CreatorId { get; set; }

    }
}
