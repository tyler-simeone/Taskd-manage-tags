using manage_tags.src.models;
using manage_tags.src.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<TagList>> GetTags(int userId)
        {
            if (_validator.ValidateGetTags(userId))
            {
                try
                {
                    TagList taskList = await _tagsRepository.GetTags(userId);
                    return Ok(taskList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("User ID is required.");
            }
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteTag(int tagId, int userId)
        {
            if (_validator.ValidateDeleteTask(tagId, userId))
            {
                try
                {
                    _tagsRepository.DeleteTag(tagId, userId);
                    return Ok("Tag Deleted");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
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