using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation.AspNetCore;
using Graphql.Demo.API.Schema.Mutations;
using Graphql.Demo.API.Schema.Queries;
using Graphql.Demo.API.Schema.Subscriptions;
using Graphql.Demo.API.Schema.Validators;
using Graphql.Demo.API.Services;
using Graphql.Demo.API.Services.DataLoaders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddGraphQLServer()
    //.RegisterDbContext<SchoolDbContext>()
    .AddQueryType<Query>()
    .AddTypeExtension<CoursesQuery>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddType<CourseType>()
    .AddType<InstructorType>()
    .AddInMemorySubscriptions()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddAuthorization();

builder.Services.AddScoped<CourseInputValidator>();
builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(o => o.AddPolicy("isAdmin", c => c.RequireClaim(FirebaseUserClaimType.EMAIL, "nifraz@live.com")));

var connectionString = builder.Configuration.GetConnectionString("sqlite");

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(x => x.UseSqlite(connectionString)
    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information, Microsoft.EntityFrameworkCore.Diagnostics.DbContextLoggerOptions.SingleLine)
    .EnableSensitiveDataLogging() //parameter values are visible. use only in development
);
builder.Services.AddDbContext<SchoolDbContext>(x => x.UseSqlite(connectionString));

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<UserDataLoader>();

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
