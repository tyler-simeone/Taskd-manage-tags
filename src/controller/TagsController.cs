using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskd_manage_tags.src.models;
using Taskd_manage_tags.src.models.errors;
using Taskd_manage_tags.src.models.requests;
using Taskd_manage_tags.src.repository;
using Taskd_manage_tags.src.util;

namespace manage_tags.src.controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        IRequestValidator _validator;
        ITagsRepository _tagsRepository;

        public TagsController(ITagsRepository tagsRepository, IRequestValidator requestValidator)
        {
            _validator = requestValidator;
            _tagsRepository = tagsRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TagList), StatusCodes.Status200OK)]
        public async Task<ActionResult<TagList>> GetTags(int userId, int boardId)
        {
            if (_validator.ValidateGetTags(userId))
            {
                try
                {
                    TagList tagList = await _tagsRepository.GetTags(userId, boardId);
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
        
        [HttpGet("task")]
        [ProducesResponseType(typeof(TagList), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskTagList>> GetTaskTags(int userId, int boardId)
        {
            if (_validator.ValidateGetTags(userId))
            {
                try
                {
                    TagList tagList = await _tagsRepository.GetTags(userId, boardId);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CreateTag(CreateTag tag)
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
        
        [HttpPost("{task}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AddTagToTask(AddTagToTask payload)
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
    }
}