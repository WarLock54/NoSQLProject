using NoSQLProject.Models;

namespace NoSQLProject.Repository
{
    public interface INeoV2Repository
    {
         Task<List<Dictionary<string, object>>> SearchPersonsByName(string searchString);
        Task<bool> AddPerson(Student student);
        Task<long> GetStudentCount();
    }
}
