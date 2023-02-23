using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _appDbContext;

    public TaskService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TaskModel> CreateTask(CreateTaskDTO newTask)
    {
        var task = new TaskModel()
        {
            Title = newTask.Title,
            Description = newTask.Description,
            Completed = newTask.Completed,
        };
        await _appDbContext.Tasks.AddAsync(task);
        await _appDbContext.SaveChangesAsync();
        return task;
    }

    public async Task<int> CountTasks(bool? type = null, string search = "")
    {
        var totalTasks = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) && (type == null || type == task.Completed)).CountAsync();
        return totalTasks;
    }

    public async Task<List<TaskModel>> GetTasks(bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var tasksList = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) && (type == null || task.Completed == type)).OrderByDescending(task => task.UpdatedAt).Skip((page - 1) * total).Take(total).ToListAsync();
        return tasksList;
    }

    public async Task<TaskModel> GetTask(Guid id)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        if (task == null)
            throw new CustomException("Not found", 404);
        return task;
    }

    public async Task<TaskModel> UpdateTask(Guid id, UpdateTaskDTO updateTask)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        if (task == null)
            throw new CustomException("Not found", 404);
        task.Title = updateTask.Title != null ? updateTask.Title : task.Title;
        task.Description = updateTask.Description != null ? updateTask.Description : task.Description;
        task.Completed = updateTask.Completed != null ? updateTask.Completed : task.Completed;
        await _appDbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskModel> DeleteTask([FromRoute] Guid id)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        if (task == null)
            throw new CustomException("Not found", 404);
        _appDbContext.Tasks.Remove(task);
        await _appDbContext.SaveChangesAsync();
        return task;
    }
}