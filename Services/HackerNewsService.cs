using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Models;
using System.Linq;
using System.Net.Http.Json;
using System;

namespace Services
{
    public class HackerNewsService
    {
        private readonly IMemoryCache _memoryCache;
        private IHackerNewsRepository _hackerNewsRepo;
        public HackerNewsService(IMemoryCache cache, IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRepo = hackerNewsRepository;
            _memoryCache = cache;

        }

        public async Task<List<HackerNewsStory>> SearchStory(string searchTerm)
        {

            List<HackerNewsStory> stories = new List<HackerNewsStory>();
            var response = await _hackerNewsRepo.BestStoriesAsync();

            if (response.IsSuccessStatusCode)
            {
                var storyIds = response.Content.ReadFromJsonAsync<List<int>>().Result;
               
               
                var tasks = storyIds.Select(GetStoryAync);
        
                stories = (await Task.WhenAll(tasks)).ToList();
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    var search = searchTerm.ToLower();
                    stories = stories.Where(
                        s => s.title.ToLower().IndexOf(search) > -1 || s.by.ToLower().IndexOf(search) > -1
                    ).ToList();

                }
            }

            return stories;

        }
        public async Task<HackerNewsStory> GetStoryAync(int storyId)
        {
            return await _memoryCache.GetOrCreateAsync<HackerNewsStory>(storyId,
                async cacheEntry =>
                {
                    cacheEntry.SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    HackerNewsStory story = new HackerNewsStory();
                    var response = await _hackerNewsRepo.GetStoryByIdAsync(storyId);
                    story = response.Content.ReadFromJsonAsync<HackerNewsStory>().Result;
                    return story;
                });

        }

        

    }

}