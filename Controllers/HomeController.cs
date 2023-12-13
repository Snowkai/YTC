using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using VideoLibrary;
using YTC.Models;
using YTC.src;

namespace YTC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string checkUrl = "https://www.youtube.com/watch?v=";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QualityView(string url)
        {
            if (url == null) {
                url = "";
            }
            if (url.Contains(checkUrl))
            {
                StaticData.currentUrl = url;
                var videoInfos = StaticData.youTube.GetAllVideosAsync(url).GetAwaiter().GetResult();
                ViewBag.Title = videoInfos.First().Title;
                ViewBag.Author = videoInfos.First().Info.Author;
                Dictionary<int, string> containsItag = new Dictionary<int, string>();
                foreach (var videoInfo in videoInfos)
                {
                    //if (videoInfo.Format == VideoFormat.Mp4)
                    //{
                        foreach (var itag in StaticData.Itag)
                        {
                            if (videoInfo.FormatCode == itag.Key)
                            {
                                containsItag.Add(itag.Key, itag.Value);
                            }
                        }
                    //}
                }
                return View("QualityView", containsItag);
            }
            else
            {
                string errUrlMessage = "This url is not youtube link!!! Please, check url.";
                return View("ErrorLink", errUrlMessage);
            }
            
        }
        [HttpPost]
        public IActionResult ProgressDownload(int? code)
        {
            var videoInfos = StaticData.youTube.GetAllVideosAsync(StaticData.currentUrl).GetAwaiter().GetResult();
            var video = videoInfos.First(x => x.FormatCode == code);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Downloads", video.FullName);
            StaticData.VideoFile = video;
            StaticData.PathFile = path;
            StaticData.PrepareFile(path, video);
            ViewBag.Title = videoInfos.First().Info.Title;
            ViewBag.totalbytes = StaticData.TotalBytes;
            return View("ProgressDownload");
        }
        [HttpPost]
        public IActionResult DownloadFile() 
        {
            string fileType = "video/mp4";
            string fileName = "";
            if(StaticData.VideoFile.FullName != null)
            {
                fileName = StaticData.VideoFile.FullName;
            }
            return PhysicalFile(StaticData.PathFile, fileType, fileName); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
