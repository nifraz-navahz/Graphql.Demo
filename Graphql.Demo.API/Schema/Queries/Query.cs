using Graphql.Demo.API.Models;
using Graphql.Demo.API.Schema.Filters;
using Graphql.Demo.API.Schema.Sorters;
using Graphql.Demo.API.Services;
using Microsoft.EntityFrameworkCore;
using System;
namespace Graphql.Demo.API.Schema.Queries
{
    public class Query
    {
        [GraphQLDeprecated("This property is deprecated.")]
        public string Instructions => "Graphql query is working! :)";
    }
}
