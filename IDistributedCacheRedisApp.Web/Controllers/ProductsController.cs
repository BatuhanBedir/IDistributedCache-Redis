using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

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
        Product product = new Product { Id = 1, Name = "Kalem1", Price = 100 };

        string jsonProduct =JsonConvert.SerializeObject(product);

        //json
        //await _distributedCache.SetStringAsync("product:1", jsonProduct, new DistributedCacheEntryOptions
        //{
        //    AbsoluteExpiration = DateTime.Now.AddMinutes(30)
        //});

        //byte
        Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
        await _distributedCache.SetAsync("product:1", byteProduct);

        return View();
    }

    public async Task<IActionResult> CacheGetString()
    {
        //byte
        Byte[] byteProduct =await _distributedCache.GetAsync("product:1");
        string jsonProduct = Encoding.UTF8.GetString(byteProduct);

        //json
        //var jsonProduct = await _distributedCache.GetStringAsync("product:1");

        Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

        ViewBag.product = product;

        return View();
    }

    public async Task<IActionResult> CacheRemove()
    {
        await _distributedCache.RemoveAsync("name");

        return View();
    }

    public IActionResult CacheFile()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Pikachu.jpg");

        byte[] imageByte = System.IO.File.ReadAllBytes(path);

        _distributedCache.Set("image", imageByte, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(30)
        });

        return View();
    }
    public IActionResult CacheFileRead()
    {
        byte[] imageByte = _distributedCache.Get("image");

        return File(imageByte, "image/jpg");
    }
}
