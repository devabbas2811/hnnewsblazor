using System.Net.Http;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        public static HttpClient httpClient =  new HttpClient();
        public HackerNewsRepository()
        {
            
        }
        public async Task<HttpResponseMessage> BestStoriesAsync()
        {

        return  await  httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            
        }
         public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await httpClient.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }
    }
}