using Microsoft.AspNetCore.Mvc;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Services.Interfaces;

public interface ITaskService
{
    public Task<TaskModel> CreateTask(CreateTaskDTO task);

    public Task<int> CountTasks(bool? type, string search);
    public Task<List<TaskModel>> GetTasks(bool? type, string search, int page, int total);
    
    public Task<TaskDTO> GetTask(Guid id);
    
    public Task<TaskModel> UpdateTask(Guid id, UpdateTaskDTO updateTask);
    
    public Task<TaskModel> DeleteTask(Guid id);
}