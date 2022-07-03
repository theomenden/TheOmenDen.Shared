namespace TheOmenDen.Shared.Extensions;
public static class DbContextExtensions
{
    public static async Task<List<T>> SqlQueryAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default)
        where T : class
    {
        parameters ??= Array.Empty<object>();

        if (typeof(T).GetProperties().Any())
        {
            await using var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction);

            db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());

            return await db2.Set<T>()
                .FromSqlRaw(sql, parameters)
                .ToListAsync(cancellationToken);

        }

        await db.Database
            .ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        return default;

    }

    private sealed class ContextForQueryType<T> : DbContext where T : class
    {
        private readonly DbConnection _connection;
        private readonly IDbContextTransaction _transaction;

        public ContextForQueryType(DbConnection connection, IDbContextTransaction tran)
        {
            _connection = connection;
            _transaction = tran;

            if (tran is not null)
            {
                Database.UseTransaction((tran as IInfrastructure<DbTransaction>)?.Instance);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_transaction != null)
            {
                optionsBuilder.UseSqlServer(_connection);

                return;
            }

            optionsBuilder.UseSqlServer(_connection, options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>()
                .HasNoKey()
                .ToView(null);
        }
    }
}
