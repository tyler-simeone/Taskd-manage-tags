using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskd_manage_tags.src.models;
using Taskd_manage_tags.src.models.errors;
using Taskd_manage_tags.src.models.requests;
using Taskd_manage_tags.src.repository;
using Taskd_manage_tags.src.util;

namespace Taskd_manage_tags.src.controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController(ITagsRepository tagsRepository, IRequestValidator requestValidator) : Controller
    {
        readonly IRequestValidator _validator = requestValidator;
        readonly ITagsRepository _tagsRepository = tagsRepository;

        #region GETs

        /// <summary>
        /// Get all tags per board. The list of available tags to add to a new task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TagList), StatusCodes.Status200OK)]
        public async Task<ActionResult<TagList>> GetAllTagsByBoardId(int userId, int boardId)
        {
            if (_validator.ValidateGetTags(userId))
            {
                try
                {
                    TagList tagList = await _tagsRepository.GetTagsByBoardId(userId, boardId);
                    return Ok(tagList);
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("User ID is required.");
            }
        }

        /// <summary>
        /// Get all tags by task ID. Will filter out any tags that have already been assigned to the Task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("task/{taskId}")]
        [ProducesResponseType(typeof(TagList), StatusCodes.Status200OK)]
        public async Task<ActionResult<TagList>> GetAvailableTagsByTaskIdAndBoardId(int taskId, int boardId)
        {
            // if (_validator.ValidateGetTags(userId))
            // {
            try
            {
                TagList tagList = await _tagsRepository.GetAvailableTagsByTaskIdAndBoardId(taskId, boardId);
                return Ok(tagList);
            }
            catch (Error ex)
            {
                Console.WriteLine($"Application Error: {ex.StackTrace}");
                var error = ErrorHelper.MapExceptionToError(ex);
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Error: {ex.Message}");
                throw;
            }
            // }
            // else
            // {
            //     return BadRequest("User ID is required.");
            // }
        }

        /// <summary>
        /// Get all tags with their parent tasks. Shows all tags tied to tasks at the board view.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("board/{boardId}")]
        [ProducesResponseType(typeof(TaskTagList), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskTagList>> GetTaskTagsByUserIdAndBoardId(int boardId, int userId)
        {
            if (_validator.ValidateGetTags(userId))
            {
                try
                {
                    TaskTagList taskTagList = await _tagsRepository.GetTaskTagsByUserIdAndBoardId(userId, boardId);
                    return Ok(taskTagList);
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("User ID is required.");
            }
        }

        #endregion GETs

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CreateTag([FromBody] CreateTag tag)
        {
            if (_validator.ValidateCreateTag(tag))
            {
                try
                {
                    var tagId = await _tagsRepository.CreateTag(tag.TagName, tag.UserId, tag.BoardId);
                    return Ok(tagId);
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("tagName and userId are required.");
            }
        }

        [HttpPost("task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AddTagToTask([FromBody] AddTagToTask payload)
        {
            if (_validator.ValidateAddTagToTask(payload))
            {
                try
                {
                    var tagId = await _tagsRepository.AddTagToTask(payload.UserId, payload.BoardId, payload.TagId, payload.TaskId);
                    return Ok(tagId);
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("userId, boardId, tagId, and taskId are required.");
            }
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteTag(int tagId, int userId)
        {
            if (_validator.ValidateDeleteTag(tagId, userId))
            {
                try
                {
                    _tagsRepository.DeleteTag(tagId, userId);
                    return Ok();
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("tagId and userId are required.");
            }
        }

        [HttpDelete("task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteTagFromTask(int taskTagId, int userId)
        {
            if (_validator.ValidateDeleteTagFromTask(taskTagId, userId))
            {
                try
                {
                    _tagsRepository.DeleteTagFromTask(taskTagId, userId);
                    return Ok();
                }
                catch (Error ex)
                {
                    Console.WriteLine($"Application Error: {ex.StackTrace}");
                    var error = ErrorHelper.MapExceptionToError(ex);
                    return BadRequest(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("taskTagId and userId are required.");
            }
        }
    }
}