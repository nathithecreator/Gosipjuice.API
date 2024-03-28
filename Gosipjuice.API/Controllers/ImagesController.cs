using Gosipjuice.API.Models.Domain;
using Gosipjuice.API.Models.DTO;
using Gosipjuice.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Gosipjuice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository) 
        {
            this.imageRepository = imageRepository;
        }

        // GET: {apibaseURL}/api/Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            // call image repository to get all images
            var images = await imageRepository.GetAll();

            //Convert Domain model to DTO
            var response = new List<BlogImageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }

            return Ok(response);
        }


        //   POST: {apibaseurl}/api/images
       [HttpPost]
       public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {

            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                //File upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //Convert Domain Model to DTO
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };

                return Ok(blogImage);
            }

            return BadRequest();
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] {".jpeg",",jpg",".png" };

            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size can not be more than 10MB");
            }

        }

    }
}
