using HWVideoCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace HWVideoCatalogue.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly string _mediaFolderPath;
        public HomeController(IWebHostEnvironment env)
        {
            _mediaFolderPath = Path.Combine(env.ContentRootPath, "MediaFiles");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var files = Directory.GetFiles(_mediaFolderPath)
                           .Select(file => new FileInfo(file));
            return View(files);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>

        public IActionResult Upload()
        {

            return View();
           
        }
       
    }
}