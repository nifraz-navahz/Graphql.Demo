using Graphql.Demo.API.Schema.Mutations;
using Graphql.Demo.API.Schema.Queries;
using Graphql.Demo.API.Schema.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseRouting();
app.UseWebSockets();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
