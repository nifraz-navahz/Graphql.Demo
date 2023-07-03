﻿using Graphql.Demo.API.Models;
using Graphql.Demo.API.Schema.Queries;

namespace Graphql.Demo.API.Schema.Mutations
{
    public class CourseInputType
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
