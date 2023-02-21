using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Controllers.api
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize(Roles = "ADMIN,USER")]
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
        public async Task<IActionResult> GetTasks([FromQuery] bool? type = null, string search = "", int page = 1, int total = 10)
        {
            var totalData = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) &&
                                                                     (type == null ? true : type == task.Completed)).CountAsync();
            var dataList = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) &&
                                                                   (type == null ? true : task.Completed == type))
                .OrderByDescending(task => task.UpdatedAt).Skip((page - 1) * total).Take(total).ToListAsync();
            return Ok(new
            {
                totalData,
                dataList
            });
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
