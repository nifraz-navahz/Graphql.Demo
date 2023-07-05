using Graphql.Demo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graphql.Demo.API.Services
{
    public class InstructorRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public InstructorRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task<Instructor?> GetById(Guid id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Instructors
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Instructor>> GetManyByIds(IEnumerable<Guid> ids)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Instructors
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
