using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using user_api.Dto;
using user_api.Models;
using user_api.Services;

namespace user_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {

        [HttpGet("enosh")]
        public ActionResult<string> Enosh() => Ok("Enosh");


        // get by id
        // delet by id
        // update by id -> user ( email, password, name )

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            var user = await userService.FindUserByIdAsync(id);
            return user == null ? NotFound($"User by the id {id} dosent exists") : Ok(user);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserModel>>> GetAll() => 
            Ok(await userService.GetAllUsersAsync());
        


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel model)
        {
            try
            {
                var user = await userService.CreateUserAsync(model);
                return Created("", user);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> Update(int id, [FromBody] UserModel user)
        {
            try
            {
                var updated = await userService.UpdateUserAsync(id, user);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> Delete(int id)
        {
            try
            {
                return Ok(await userService.DeleteUserAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<string>> Auth([FromBody] LoginDto loginDto)
        {
            try
            {
                return await userService.AuthenticateUserAsync(loginDto.Email, loginDto.Password)
                    ? Ok("Authenticated successfully")
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
