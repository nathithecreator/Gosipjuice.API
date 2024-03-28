using Gosipjuice.API.Models.Domain;

namespace Gosipjuice.API.Repositories.Interface
{
    public interface IImageRepository
    {
       Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    
        Task<IEnumerable<BlogImage>> GetAll();
    }
}
