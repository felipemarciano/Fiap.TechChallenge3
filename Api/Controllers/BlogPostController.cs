using Api.Model;
using ApplicationCore.Aggregates;
using ApplicationCore.Constants;
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
    public class BlogPostController : ControllerBase
    {
        private readonly ILogger<BlogPostController> _logger;
        private readonly IPublicationService _publicationService;
        private readonly IReadRepository<BlogPost> _blogPostRepository;
        private readonly IReadRepository<Profile> _profileRepository;

        public BlogPostController(ILogger<BlogPostController> logger,
            IReadRepository<BlogPost> blogPostRepository, IPublicationService publicationService, IReadRepository<Profile> profileRepository)
        {
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _publicationService = publicationService;
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BlogPostResponse>))]
        public async Task<IActionResult> Get()
        {
            var list = await _blogPostRepository.ListAsync(new BlogPostWithCommentsSpecification());

            return Ok(list.Select(post => new BlogPostResponse
            {
                Id = post.Id,
                Author = post.Author,
                Content = post.Content,
                Title = post.Title,
                Comments = post.Comments.Select(comment => new CommentResponse
                {
                    Id = comment.Id,
                    Text = comment.Text
                }).ToList()
            }).ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogPostResponse))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var blogPost = await _blogPostRepository.FirstOrDefaultAsync(new BlogPostWithCommentsSpecification(id));

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(new BlogPostResponse
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                Title = blogPost.Title,
                Comments = blogPost.Comments.Select(comment => new CommentResponse
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    BlogPostId = comment.BlogPostId,
                    ProfileId = comment.ProfileId                   
                }).ToList()
            });
        }

        [HttpPost]
        [Authorize(Roles = RolesConstants.ADMINISTRATORS)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] BlogPostRequest blogPostRequest)
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

                await _publicationService.PublishBlogPost(profile.Id, blogPostRequest.Title, blogPostRequest.Content, blogPostRequest.Author);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
