using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Graphql.Demo.API.Schema.Queries;

namespace Graphql.Demo.API.Services.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<string, UserType>
    {
        private readonly FirebaseAuth firebaseAuth;
        private const int FIREBASE_MAX_IDENTIFIERS = 100;
        public UserDataLoader(FirebaseApp firebaseApp, IBatchScheduler batchScheduler) : base(batchScheduler, new DataLoaderOptions { MaxBatchSize = FIREBASE_MAX_IDENTIFIERS })
        {
            firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(IReadOnlyList<string> userIds, CancellationToken cancellationToken)
        {
            var uidIdentifiers = userIds.Select(x => new UidIdentifier(x));
            var userResult = await firebaseAuth.GetUsersAsync(uidIdentifiers.ToList());

            return userResult.Users.Select(x => new UserType
            {
                Id = x.Uid,
                Username = x.DisplayName,
                PhotoUrl = x.PhotoUrl
            }).ToDictionary(x => x.Id);
        }
    }
}
