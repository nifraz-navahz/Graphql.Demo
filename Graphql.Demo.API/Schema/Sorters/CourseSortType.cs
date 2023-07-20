﻿using Graphql.Demo.API.Schema.Queries;
using HotChocolate.Data.Sorting;

namespace Graphql.Demo.API.Schema.Sorters
{
    public class CourseSortType: SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(x => x.Id);
            descriptor.Ignore(x => x.InstructorId);
            //descriptor.Field(x => x.Name).Name("CourseName");

            base.Configure(descriptor);
        }
    }
}
