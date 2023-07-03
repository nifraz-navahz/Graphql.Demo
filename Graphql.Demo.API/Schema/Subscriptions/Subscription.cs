﻿using Graphql.Demo.API.Schema.Mutations;
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

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }
    }
}
