using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Controllers.api
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public TaskController(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDTO newTask)
        {
            var task = new TaskModel()
            {
                Title = newTask.Title,
                Description = newTask.Description,
                Completed = newTask.Completed,
            };
            await _appDbContext.Tasks.AddAsync(task);
            await _appDbContext.SaveChangesAsync();
            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _appDbContext.Tasks.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTask([FromRoute] Guid id)
        {
            var task = await _appDbContext.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, UpdateTaskDTO updateTask)
        {
            var task = await _appDbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = updateTask.Title != null ? updateTask.Title : task.Title;
            task.Description = updateTask.Description != null ? updateTask.Description : task.Description;
            task.Completed = updateTask.Completed != null ? updateTask.Completed : task.Completed;

            await _appDbContext.SaveChangesAsync();
            return Ok(task);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            var task = await _appDbContext.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            _appDbContext.Tasks.Remove(task);
            await _appDbContext.SaveChangesAsync();
            return Ok(task);
        }
    }
}
