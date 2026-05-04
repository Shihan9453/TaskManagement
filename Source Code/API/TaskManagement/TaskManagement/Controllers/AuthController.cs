using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagement.Helpers;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly AppConstants _appConstants;

        public AuthController(AppDbContext appDbContext, AppConstants appConstants)
        {
            _appDbContext = appDbContext;
            _appConstants = appConstants;
        }



        /// <summary>User login</summary>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="401">UNAUTHORIZED</response> 
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                if (string.IsNullOrEmpty(loginUserDto.Username) == true && string.IsNullOrEmpty(loginUserDto.UserPassword) == true)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, _appConstants.ERROR_USERNAME_PASSWORD_REQUIRED);
                    return BadRequest(error);
                }

                var user = _appDbContext.Users.FirstOrDefault(u => u.Username.ToString().Trim() == loginUserDto.Username.ToString().Trim());
                if (user is not null)
                {
                    if (user.UserPassword.ToString().Trim() == loginUserDto.UserPassword.ToString().Trim())
                    {
                        var userResponseDto = new UserResponseDto { Id = user.Id, Username = user.Username, UserPassword = user.UserPassword, FullName = user.FullName };
                        var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_LOGIN_SUCCESS, userResponseDto);
                        return Ok(success);
                    }
                    else
                    {
                        var error = new ApiResponse(_appConstants.STATUS_CODE_UNAUTHORIZED, _appConstants.STATUS_UNAUTHORIZED, _appConstants.ERROR_PASSWORD_INVALID);
                        return Unauthorized(error);
                    }
                }
                else
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_UNAUTHORIZED, _appConstants.STATUS_UNAUTHORIZED, _appConstants.ERROR_USERNAME_INVALID);
                    return Unauthorized(error);
                }
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }

    }
}
