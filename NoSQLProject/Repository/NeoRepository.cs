using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using Neo4jClient;
using Neo4jClient.Cypher;
using NoSQLProject.Models;

namespace NoSQLProject.Repository
{
    public class NeoRepository : INeoRepository
    {
        private readonly BoltGraphClient clientNeo;
        public NeoRepository()
        {
            clientNeo = new BoltGraphClient(new Uri("bolt+s://4f93cdbe.databases.neo4j.io:7687"), "neo4j", "root");
        }

        async void INeoRepository.Assign(int did, int eid)
        {
            await clientNeo.Cypher.Match("(m:Movie),(p:Person)").Where((Movie movie, Person person) => movie.Id == did && person.Id == eid).Create("(d)-[r:hasPerson->(d)]").ExecuteWithoutResultsAsync();
        }

        void INeoRepository.Create(Movie movie)
        {
            clientNeo.Cypher.Create("(m:Movie $movie)").WithParam("movie", movie).ExecuteWithoutResultsAsync();
           
        }

        void INeoRepository.Delete(int Id)
        {
           clientNeo.Cypher.Delete("(m:Movie)").Where((Movie movie)=>movie.Id==Id).Delete("m").ExecuteWithoutResultsAsync();
            
        }

         void INeoRepository.fetchbyname(string name)
        {
            clientNeo.Cypher.Match("(m:Movie)").Where((Movie movie)=>movie.Name==name).Return(m=>m.As<Movie>());
        }

        void INeoRepository.Get()
        {
            clientNeo.Cypher.Match("m:Movie").Return(m => m.As<Movie>());
        }

        async void INeoRepository.Update(int Id, Movie movie)
        {
          await  clientNeo.Cypher.Match("m:Movie").Where((Movie movie)=>movie.Id==Id).Set("m = $movie").WithParam("movie",movie).ExecuteWithoutResultsAsync();
        }
    }
}
