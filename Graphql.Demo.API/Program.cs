using Graphql.Demo.API.Schema.Mutations;
using Graphql.Demo.API.Schema.Queries;
using Graphql.Demo.API.Schema.Subscriptions;
using Graphql.Demo.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

var connectionString = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(x => x.UseSqlite(connectionString));
builder.Services.AddScoped<CourseRepository>();

var app = builder.Build();

app.UseRouting();
app.UseWebSockets();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

using (var scope = app.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
    using var context = contextFactory.CreateDbContext();
    context.Database.Migrate();
}

app.Run();
