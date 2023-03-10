using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public TaskService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<TaskModel> CreateTask(CreateTaskDTO newTask)
    {
        using (IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var task = new TaskModel()
                {
                    Title = newTask.Title,
                    Description = newTask.Description,
                    Completed = newTask.Completed,
                };
                await _appDbContext.Tasks.AddAsync(task);
                foreach (var user in newTask.Users)
                {
                    await _appDbContext.UserTask.AddAsync(new UserTaskModel()
                    {
                        TaskId = task.Id,
                        UserId = user.Id
                    });
                }
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return task;
            }
            catch (Exception error)
            {
                 await transaction.RollbackAsync();
                throw new CustomException(error.Message, 400);
            }
        }

        
    }

    public async Task<int> CountTasks(bool? type = null, string search = "")
    {
        var totalTasks = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) && (type == null || type == task.Completed)).CountAsync();
        return totalTasks;
    }

    public async Task<List<TaskModel>> GetTasks(bool? type = null, string search = "", int page = 1, int total = 10)
    {
        System.Diagnostics.Debug.WriteLine("TOTAL " + total + " " + page);
        var tasksList = await _appDbContext.Tasks.Where(task => task.Title.Contains(search) && (type == null || task.Completed == type)).OrderByDescending(task => task.CreatedAt).Skip((page - 1) * total).Take(total).ToListAsync();
        return tasksList;
    }

    public async Task<TaskDTO> GetTask(Guid id)
    {
        var task = await _appDbContext.Tasks
            .Where(task => task.Id == id)
            .Include(task => task.UserTasks)
            .ThenInclude(userTask => userTask.User)
            .FirstOrDefaultAsync();
            
        if (task == null)
            throw new CustomException("Not found", 404);
        var taskResponse = _mapper.Map<TaskDTO>(task);
        return taskResponse;
    }

    public async Task<TaskModel> UpdateTask(Guid id, UpdateTaskDTO updateTask)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        if (task == null)
            throw new CustomException("Not found", 404);
        
        // task.Title = updateTask.Title != null ? updateTask.Title : task.Title;
        // task.Description = updateTask.Description != null ? updateTask.Description : task.Description;
        // task.Completed = updateTask.Completed != null ? updateTask.Completed : task.Completed;
        
        task = _mapper.Map<UpdateTaskDTO, TaskModel>(updateTask, task);
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