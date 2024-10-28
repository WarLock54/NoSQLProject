using MongoDB.Bson;
using Neo4jClient.Cypher;
using NoSQLProject.Models;

namespace NoSQLProject.Repository
{
    public interface INeoRepository
    {
        void  Create(Movie movie);

        // Read
        void Get();
        void  fetchbyname(string name);

        // Update
        void Update(int Id, Movie movie);

        // Delete
        void Delete(int Id);

        void Assign(int did, int eid);
    }
}
