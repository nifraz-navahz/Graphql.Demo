using Graphql.Demo.API.Schema.Mutations;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using System;
namespace Graphql.Demo.API.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        //[Topic("courseCreated")]
        public CourseResult CourseCreated([EventMessage]CourseResult course) => course;

        [Subscribe(With = nameof(SubscribeCourseUpdated))]
        public CourseResult CourseUpdated([EventMessage] CourseResult book) => book;

        public ValueTask<ISourceStream<CourseResult>> SubscribeCourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver) 
        {
            var topicName = $"{courseId}_{nameof(CourseUpdated)}";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }
    }
}
