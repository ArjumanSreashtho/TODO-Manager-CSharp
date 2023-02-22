﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "USER,ADMIN")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly HashingPassword _hashingPassword;
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
    {
        var user = await _userService.CreateUser(newUser);
        if (user == null)
            return BadRequest(new
            {
                success = false,
                message = "Username already exists",
            });
        return Ok(new
        {
            success = true,
            message = "User has been created",
            data = user
        });
    }
    
    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUsers([FromQuery] bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var totalUsers = await _userService.CountUsers(type, search);
        var userList = await _userService.GetUsers(type, search, page, total);
        return Ok(new
        {
            success = true,
            message = "Tasks have been retrieved",
            data = new
            {
                total = totalUsers,
                list = userList
            }
        });
    }
}