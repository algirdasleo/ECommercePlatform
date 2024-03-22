using Microsoft.AspNetCore.Mvc;
using User.Models;
using User.Services;
using SharedLibrary.Interfaces;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDBService<UserItem> _userService;

        public UserController(IDBService<UserItem> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(){
            var users = await _userService.GetAllAsync();
            if (!users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id){
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserItem user){
            var userId = (await _userService.CreateAsync(user)).UserId;
            user.UserId = userId;
            var actionName = nameof(GetUserById);
            var routeValues = new { id = userId };
            return CreatedAtAction(actionName, routeValues, user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserItem user){
            var updatedUser = await _userService.UpdateAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id){
            var deletedUser = await _userService.DeleteAsync(id);
            return Ok(deletedUser);
        }
    }
}