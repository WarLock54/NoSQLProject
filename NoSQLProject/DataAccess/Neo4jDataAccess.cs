
using Amazon.Util.Internal.PlatformServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace NoSQLProject.DataAccess
{
    public class Neo4jDataAccess : INeo4jDataAccess
    {
        private readonly IAsyncSession _session;

        private readonly ILogger<Neo4jDataAccess> _logger;
        private string _database;
        public Neo4jDataAccess(IDriver driver, ILogger<Neo4jDataAccess> logger, IOptions<ApplicationSettings> options)
        {
            _logger = logger;
            _database = "neo4j";
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _session.CloseAsync();
        }

        async Task<List<Dictionary<string, object>>> INeo4jDataAccess.ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {
            return await ExecuteReadTransactionAsync<Dictionary<string, object>>(query, returnObjectKey, parameters);
        }

        async Task<List<string>> INeo4jDataAccess.ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {
            return await ExecuteReadTransactionAsync<string>(query, returnObjectKey, parameters);
        }

        public async Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters = parameters == null ? new Dictionary<string, object>() : parameters;

                var result = await _session.ReadTransactionAsync(async tx =>
                {
                    T scalar = default(T);
                    var res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        /// <summary>
        /// Execute write transaction
        /// </summary>
        public async Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters = parameters == null ? new Dictionary<string, object>() : parameters;

                var result = await _session.WriteTransactionAsync(async tx =>
                {
                    T scalar = default(T);
                    var res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        /// <summary>
        /// Execute read transaction as an asynchronous operation.
        /// </summary>
        private async Task<List<T>> ExecuteReadTransactionAsync<T>(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {
            try
            {
                parameters = parameters == null ? new Dictionary<string, object>() : parameters;

                var result = await _session.ReadTransactionAsync(async tx =>
                {
                    var data = new List<T>();
                    var res = await tx.RunAsync(query, parameters);
                    var records = await res.ToListAsync();
                    data = records.Select(x => (T)x.Values[returnObjectKey]).ToList();
                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }
    }
}
