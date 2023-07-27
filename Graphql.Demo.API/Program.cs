using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using Graphql.Demo.API.Schema.Mutations;
using Graphql.Demo.API.Schema.Queries;
using Graphql.Demo.API.Schema.Subscriptions;
using Graphql.Demo.API.Services;
using Graphql.Demo.API.Services.DataLoaders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(o => o.AddPolicy("isAdmin", c => c.RequireClaim(FirebaseUserClaimType.EMAIL, "nifraz@live.com")));

var connectionString = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(x => x.UseSqlite(connectionString));

builder.Services.AddDbContext<SchoolDbContext>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorDataLoader>();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
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
