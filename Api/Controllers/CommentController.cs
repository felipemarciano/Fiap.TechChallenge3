using Api.Model;
using ApplicationCore.Aggregates;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly IPublicationService _publicationService;
        private readonly IReadRepository<Profile> _profileRepository;

        public CommentController(ILogger<CommentController> logger,
           IPublicationService publicationService, IReadRepository<Profile> profileRepository)
        {
            _logger = logger;
            _publicationService = publicationService;
            _profileRepository = profileRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CommentRequest commentRequest)
        {
            try
            {
                string? userId = User?.FindFirst(ClaimTypes.Name)?.Value;

                if (userId == null)
                {
                    return Unauthorized();
                }

                var profile = await _profileRepository.FirstOrDefaultAsync(new ProfileSpecification(Guid.Parse(userId)));

                if (profile == null)
                {
                    return Unauthorized();
                }


                await _publicationService.AddCommentToBlogPost(new Comment(profile.Id, commentRequest?.Text ?? "", commentRequest?.BlogPostId ?? Guid.Empty));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
