using Graphql.Demo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graphql.Demo.API.Services
{
    public class CourseRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public CourseRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Courses
                .Include(x => x.Instructor)
                .Include(x => x.Students)
                .ToListAsync();
        }

        public async Task<Course?> GetById(Guid id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Courses
                .Include(x => x.Instructor)
                .Include(x => x.Students)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Course> Create(Course course)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> Update(Course course)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Courses.Update(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> Delete(Guid id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var course = new Course { Id = id };
            context.Courses.Remove(course);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
