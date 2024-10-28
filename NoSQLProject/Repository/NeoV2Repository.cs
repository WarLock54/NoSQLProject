using NoSQLProject.DataAccess;
using NoSQLProject.Models;
using System;

namespace NoSQLProject.Repository
{
    public class NeoV2Repository: INeoV2Repository
    {
        private readonly INeo4jDataAccess _neo4jDataAccess;

        private readonly ILogger<NeoV2Repository> _logger;
        public NeoV2Repository(INeo4jDataAccess neo4jDataAccess, ILogger<NeoV2Repository> logger)
        {
            _neo4jDataAccess = neo4jDataAccess;
            _logger = logger;
        }
        public async Task<List<Dictionary<string, object>>> SearchPersonsByName(string searchString)
        {
            var query = @"MATCH (s:Student) where toUpper(s.name) CONTAINS toUpper($searchString) RETURN s{name:s.name,ClassName:s.ClassName} ORDER BY s.Name LIMIT 5";
            IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", searchString } };
            var students = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "s", parameters);
            return students;
        }
        public async Task<bool> AddPerson(Student student)
        {
            if(student != null && string.IsNullOrWhiteSpace(student.name))
            {
                var query = @"MERGE (s:Student {name: $name}) ON CREATE SET s.year= $year ON MATCH SET s.year=$year RETURN true";
                IDictionary<string, object> parameters = new Dictionary<string, object> { { "name", student.name }, { "year", student.year }, { "className", student.className } };
                return await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(query, parameters);
            }
            else
            {
                throw new System.ArgumentNullException(nameof(student), "Person must not be null");
            }
        }
        public async Task<long> GetStudentCount()
        {
            var query = @"MATCH (s:Student) RETURN count(s) as studentCount";
            var count=await _neo4jDataAccess.ExecuteReadScalarAsync<long>(query);
            return count;
        }
    }
}
