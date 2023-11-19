using task.Models;
using task.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace task.Services
{
    public class TaskServices : ITaskService
    {
        List<Task> Tasks { get; }
        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskServices(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");

            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll() => Tasks;

        public Task Get(int id) => Tasks.FirstOrDefault(t => t.id == id);

        public void Add(Task tasks)
        {
            tasks.id = Tasks.Count() + 1;
            Tasks.Add(tasks);
            saveToFile();
        }

        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }

        public void Update(Task task)
        {
            var index = Tasks.FindIndex(p => p.id == task.id);
            if (index == -1)
                return;

            Tasks[index] = task;
            saveToFile();
        }

        public int Count => Tasks.Count();
    }
}
