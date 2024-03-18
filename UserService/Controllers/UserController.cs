using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;
using SharedLibrary.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDBService<User> _userService;

        public UserController(IDBService<User> userService)
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
        public async Task<IActionResult> CreateUser(User user){
            var userId = (await _userService.CreateAsync(user)).UserId;
            user.UserId = userId;
            var actionName = nameof(GetUserById);
            var routeValues = new { id = userId };
            return CreatedAtAction(actionName, routeValues, user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user){
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