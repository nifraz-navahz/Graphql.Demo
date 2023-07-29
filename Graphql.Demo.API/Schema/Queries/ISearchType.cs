namespace Graphql.Demo.API.Schema.Queries
{
    [InterfaceType("SearchType")]
    //[UnionType("SearchType")] //when no shared properties
    public interface ISearchType
    {
        public Guid Id { get; set; }
    }
}
