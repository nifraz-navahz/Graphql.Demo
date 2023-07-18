using Graphql.Demo.API.Schema.Queries;
using HotChocolate.Data.Filters;

namespace Graphql.Demo.API.Schema.Filters
{
    public class CourseFilterType: FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(x => x.Students);
            base.Configure(descriptor);
        }
    }
}
