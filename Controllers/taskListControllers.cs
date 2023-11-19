using System;
using System.Collections.Generic;
using System.Linq;
// using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task.Models;
using task.Interfaces;

namespace task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class taskListControllers : ControllerBase
    {

        ITaskService TaskServices;

        public taskListControllers(ITaskService TaskServices)
        {

            this.TaskServices = TaskServices;

        }

        [HttpGet]
        public ActionResult<List<Task>> GetAll() => TaskServices.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskServices.Get(id);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        public ActionResult Create(Task task)
        {
            TaskServices.Add(task);
            return CreatedAtAction(nameof(Create), new { id = task.id }, task);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.id)
                return BadRequest();

            var existingTask = TaskServices.Get(id);
            if (existingTask is null)
                return NotFound();

            TaskServices.Update(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskServices.Get(id);
            if (task is null)
                return NotFound();

            TaskServices.Delete(id);

            return Content(TaskServices.Count.ToString());
        }
    }
}
