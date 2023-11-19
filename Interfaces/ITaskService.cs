using task.Models;
using System.Collections.Generic;
// using lesson_1;

namespace task.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll();
        Task Get(int id);
        void Add(Task task);
        void Delete(int id);
        void Update(Task task);
        int Count {get;}

    }  

}
