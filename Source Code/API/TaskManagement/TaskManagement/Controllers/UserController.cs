using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Helpers;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly AppConstants _appConstants;

        public UserController(AppDbContext appDbContext, AppConstants appConstants)
        {
            _appDbContext = appDbContext;
            _appConstants = appConstants;
        }



        /// <summary>Get all users</summary>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var lstUsers = _appDbContext.Users.AsNoTracking().ToList();
                if (lstUsers.Count() == 0)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND);
                    return NotFound(error);
                }
                else
                {
                    var userResponseDto = lstUsers.Select(u => new UserResponseDto { Id = u.Id, Username = u.Username, UserPassword = u.UserPassword, FullName = u.FullName }).ToList();
                    var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_RECORDS_FOUND, userResponseDto);
                    return Ok(success);
                }
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Save user</summary>
        /// <response code="201">CREATED</response>
        /// <response code="400">BAD REQUEST</response> 
        [HttpPost("SaveUser")]
        public IActionResult SaveUser([FromBody] AddUserDto addUserDto)
        {
            try
            {
                var user = new User()
                {
                    Username = addUserDto.Username,
                    UserPassword = addUserDto.UserPassword,
                    FullName = addUserDto.FullName
                };

                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();

                var userResponseDto = new UserResponseDto { Id = user.Id, Username = user.Username, UserPassword = user.UserPassword, FullName = user.FullName };
                var success = new ApiResponse(_appConstants.STATUS_CODE_CREATED, _appConstants.STATUS_CREATED, _appConstants.SUCCESS_INSERTED, userResponseDto);
                return Ok(success);
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Delete user</summary>
        /// <param name="Id">User Id</param>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpDelete("DeleteUser/{Id:int}")]
        public IActionResult DeleteUser(int Id)
        {
            try
            {
                var user = _appDbContext.Users.Find(Id);
                if (user is null)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND_TO_DELETE);
                    return NotFound(error);
                }
                else
                {
                    _appDbContext.Users.Remove(user);
                    _appDbContext.SaveChanges();

                    var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_DELETED);
                    return Ok(success);
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
