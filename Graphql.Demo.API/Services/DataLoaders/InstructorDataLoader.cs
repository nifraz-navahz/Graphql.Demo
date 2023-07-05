﻿using Graphql.Demo.API.Entities;

namespace Graphql.Demo.API.Services.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, Instructor>
    {
        private readonly InstructorRepository _instructorRepository;
        public InstructorDataLoader(InstructorRepository instructorRepository, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            _instructorRepository = instructorRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Instructor>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var instructors = await _instructorRepository.GetManyByIds(keys);
            return instructors.ToDictionary(x => x.Id);
        }
    }
}
