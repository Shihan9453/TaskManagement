using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Helpers;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{

    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    [Route("api/[Controller]")]
    public class TaskController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly AppConstants _appConstants;

        public TaskController(AppDbContext appDbContext, AppConstants appConstants)
        {
            _appDbContext = appDbContext;
            _appConstants = appConstants;
        }



        /// <summary>Get all tasks</summary>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpGet("GetTasks")]
        public IActionResult GetTasks()
        {
            try
            {
                var lstTasks = _appDbContext.Tasks.AsNoTracking().Select(t => new { t.Id, t.Title, t.Description, t.Status, t.PriorityLevel, t.AssignedPersonId, AssignedPersonName = t.AssignedPerson.FullName, t.DueDate, t.CompletedDate, t.Remarks }).ToList();
                if (lstTasks.Count() == 0)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND);
                    return NotFound(error);
                }
                else
                {
                    var taskResponseDto = lstTasks.Select(t => new TaskResponseDto { Id = t.Id, Title = t.Title, Description = t.Description, Status = t.Status, PriorityLevel = t.PriorityLevel, AssignedPerson = t.AssignedPersonName, AssignedPersonId = t.AssignedPersonId, DueDate = t.DueDate, CompletedDate = t.CompletedDate, Remarks = t.Remarks }).ToList();
                    var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_RECORDS_FOUND, taskResponseDto);
                    return Ok(success);
                }
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Get a task</summary>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpGet("GetTask/{Id:int}")]
        public IActionResult GetTask(int Id)
        {
            try
            {
                var task = _appDbContext.Tasks.AsNoTracking().Select(t => new { t.Id, t.Title, t.Description, t.Status, t.PriorityLevel, t.AssignedPersonId, AssignedPersonName = t.AssignedPerson.FullName, t.DueDate, t.CompletedDate, t.Remarks }).FirstOrDefault(t => t.Id == Id);
                if (task is null)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND);
                    return NotFound(error);
                }
                else
                {
                    var taskResponseDto = new TaskResponseDto { Id = task.Id, Title = task.Title, Description = task.Description, Status = task.Status, PriorityLevel = task.PriorityLevel, AssignedPerson = task.AssignedPersonName, AssignedPersonId = task.AssignedPersonId, DueDate = task.DueDate, CompletedDate = task.CompletedDate, Remarks = task.Remarks };
                    var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_RECORDS_FOUND, taskResponseDto);
                    return Ok(success);
                }
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Save task</summary>
        /// <response code="201">CREATED</response> 
        /// <response code="400">BAD REQUEST</response> 
        [HttpPost("SaveTask")]
        public IActionResult SaveTask([FromBody] AddTaskDto addTaskDto)
        {
            try
            {
                var task = new Models.Task()
                {
                    Title = addTaskDto.Title,
                    Description = addTaskDto.Description,
                    Status = addTaskDto.Status,
                    PriorityLevel = addTaskDto.PriorityLevel,
                    AssignedPersonId = addTaskDto.AssignedPersonId,
                    DueDate = addTaskDto.DueDate
                };

                _appDbContext.Tasks.Add(task);
                _appDbContext.SaveChanges();

                int intLoggedUserId = GetLoggedUserId();
                if (intLoggedUserId > 0)
                {
                    var taskHistory = new TasksHistory()
                    {
                        TaskId = task.Id,
                        UserId = intLoggedUserId,
                        Action = _appConstants.TASK_HISTORY_ACTION_INSERTED + " (Task Status: " + addTaskDto.Status + ")",
                        Date = DateTime.Now.Date
                    };
                    _appDbContext.TasksHistories.Add(taskHistory);
                    _appDbContext.SaveChanges();
                }

                var taskResponseDto = new TaskResponseDto { Id = task.Id, Title = task.Title, Description = task.Description, Status = task.Status, PriorityLevel = task.PriorityLevel, AssignedPersonId = task.AssignedPersonId, DueDate = task.DueDate};
                var success = new ApiResponse(_appConstants.STATUS_CODE_CREATED, _appConstants.STATUS_CREATED, _appConstants.SUCCESS_INSERTED, taskResponseDto);
                return Created("", success);
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Edit task</summary>
        /// <param name="Id">Task Id</param>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpPut("EditTask/{Id:int}")]
        public IActionResult EditTask(int Id,[FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                var task = _appDbContext.Tasks.Find(Id);
                if (task is null)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND_TO_UPDATE);
                    return NotFound(error);
                }
                else
                {                    
                    task.Title = updateTaskDto.Title;
                    task.Description = updateTaskDto.Description;
                    task.Status = updateTaskDto.Status;
                    task.PriorityLevel = updateTaskDto.PriorityLevel;
                    task.AssignedPersonId = updateTaskDto.AssignedPersonId;
                    task.DueDate = updateTaskDto.DueDate;
                    task.CompletedDate = updateTaskDto.CompletedDate;
                    task.Remarks = updateTaskDto.Remarks;
                  
                    _appDbContext.Tasks.Update(task);
                    _appDbContext.SaveChanges();

                    int intLoggedUserId = GetLoggedUserId();
                    if (intLoggedUserId > 0)
                    {
                        var taskHistory = new TasksHistory()
                        {
                            TaskId = task.Id,
                            UserId = intLoggedUserId,
                            Action = _appConstants.TASK_HISTORY_ACTION_UPDATED + " (Task Status: " + updateTaskDto.Status + ")",
                            Date = DateTime.Now.Date
                        };
                        _appDbContext.TasksHistories.Add(taskHistory);
                        _appDbContext.SaveChanges();
                    }

                    var taskResponseDto = new TaskResponseDto { Id = Id, Title = task.Title, Description = task.Description, Status = task.Status, PriorityLevel = task.PriorityLevel, AssignedPersonId = task.AssignedPersonId, DueDate = task.DueDate.Date, CompletedDate = task.CompletedDate, Remarks = task.Remarks };
                    var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_UPDATED, taskResponseDto);
                    return Ok(success);
                }
            }
            catch (Exception ex)
            {
                var error = new ApiResponse(_appConstants.STATUS_CODE_BAD_REQUEST, _appConstants.STATUS_BAD_REQUEST, ex.InnerException?.Message ?? ex.Message);
                return BadRequest(error);
            }
        }



        /// <summary>Delete task</summary>
        /// <param name="Id">Task Id</param>
        /// <response code="200">OK</response> 
        /// <response code="400">BAD REQUEST</response> 
        /// <response code="404">NOT FOUND</response> 
        [HttpDelete("DeleteTask/{Id:int}")]
        public IActionResult DeleteTask(int Id)
        {
            try
            {
                var task = _appDbContext.Tasks.Find(Id);
                if (task is null)
                {
                    var error = new ApiResponse(_appConstants.STATUS_CODE_NOT_FOUND, _appConstants.STATUS_NOT_FOUND, _appConstants.ERROR_NO_RCORDS_FOUND_TO_DELETE);
                    return NotFound(error);
                }
                else
                {
                    var taskHistories = _appDbContext.TasksHistories.Where(t => t.TaskId == Id);
                    _appDbContext.TasksHistories.RemoveRange(taskHistories);
                    _appDbContext.SaveChanges();

                    _appDbContext.Tasks.Remove(task);
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



        /// <summary>Get task status</summary>
        /// <response code="200">OK</response> 
        [HttpGet("GetTaskStatuses")]
        public IActionResult GetTaskStatuses()
        {
            var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_RECORDS_FOUND, _appConstants.lstStatuses);
            return Ok(success);
        }



        /// <summary>Get task priority levels</summary>
        /// <response code="200">OK</response> 
        [HttpGet("GetTaskPriorityLevels")]
        public IActionResult GetTaskPriorityLevels()
        {
            var success = new ApiResponse(_appConstants.STATUS_CODE_OK, _appConstants.STATUS_OK, _appConstants.SUCCESS_RECORDS_FOUND, _appConstants.lstPriorityLevels);
            return Ok(success);
        }



        // Get Logged User Id
        private int GetLoggedUserId()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException(_appConstants.ERROR_USER_NOT_FOUND);
            }
            return int.Parse(userId);
        }



    }
}
