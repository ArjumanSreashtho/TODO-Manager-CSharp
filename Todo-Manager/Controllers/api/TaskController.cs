using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_Manager.DTO.Task;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService) 
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDTO newTask)
        {
            var task = await _taskService.CreateTask(newTask);
            return Ok(new
            {
                message = "Task has been created",
                data = task
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] bool? type = null, string search = "", int page = 1, int total = 10)
        {
            var totalTasks = await _taskService.CountTasks(type, search);
            var tasksList = await _taskService.GetTasks(type, search, page, total);
            
            return Ok(new
            {
                message = "Tasks have been retrieved",
                data = new
                {
                    total = totalTasks,
                    list = tasksList
                }
            });
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTask([FromRoute] Guid id)
        {
            var task = await _taskService.GetTask(id);
            return Ok(new
            {
                message = "Task has been retrieved",
                data = task
            });
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, UpdateTaskDTO updateTask)
        {
            var task = await _taskService.UpdateTask(id, updateTask);
            return Ok(new
            {
                message = "Task has been updated",
                data = task
            });
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            await _taskService.DeleteTask(id);
            return Ok(new
            {
                message = "Task has been deleted",
            });
        }
    }
}
