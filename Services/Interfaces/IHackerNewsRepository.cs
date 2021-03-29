using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace Services.Interfaces
{
    public interface IHackerNewsRepository
    {
        Task<HttpResponseMessage> BestStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);


        
    }
}