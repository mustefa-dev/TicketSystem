using Microsoft.AspNetCore.Authorization;
using TicketSystem.DATA.DTOs.User;
using TicketSystem.Services;
using TicketSystem.Utils;
using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Controllers;
// [Authorize(Roles = "Admin")]

public class UsersController : BaseController{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) {
        _userService = userService;
    }
    [AllowAnonymous]

    [HttpPost("/api/Login")]
    public async Task<ActionResult> Login(LoginForm loginForm) => Ok(await _userService.Login(loginForm));
    
    [AllowAnonymous]

    [HttpPost("/api/Users")]
    public async Task<ActionResult> Create(RegisterForm registerForm) =>
        Ok(await _userService.Register(registerForm));


    [HttpGet("/api/Users/{id}")]
    public async Task<ActionResult> GetById(Guid id) => OkObject(await _userService.GetUserById(id));

    [HttpPut("/api/Users/{id}")]
    public async Task<ActionResult> Update(UpdateUserForm updateUserForm, Guid id) =>
        Ok(await _userService.UpdateUser(updateUserForm, id));

    [HttpDelete("/api/Users/{id}")]
    public async Task<ActionResult> Delete(Guid id) => Ok(await _userService.DeleteUser(id));


    [HttpGet("/api/Users")]
    public async Task<ActionResult<Respons<UserDto>>> GetAll([FromQuery] UserFilter filter) =>
        Ok(await _userService.GetAll(filter), filter.PageNumber, filter.PageSize);
}