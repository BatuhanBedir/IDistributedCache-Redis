using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController : Controller
{
    private IDistributedCache _distributedCache;

    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<IActionResult> CacheSetString()
    {
        await _distributedCache.SetStringAsync("name", "Batuhan", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        }) ;

        return View();
    }

    public async Task<IActionResult> CacheGetString()
    {
        var name =await _distributedCache.GetStringAsync("name");

        ViewBag.name = name;

        return View();
    }

    public async Task<IActionResult> CacheRemove()
    {
        await _distributedCache.RemoveAsync("name");

        return View();
    }
}
