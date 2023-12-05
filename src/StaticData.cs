using System.Net;
using VideoLibrary;

namespace YTC.src
{
    public static class StaticData
    {
        public static Dictionary<int, string> Itag = new Dictionary<int, string>()
        {
            //{ 133, "240p" },
            { 134, "360p video only" },
            { 135, "480p video only" },
            { 136, "720p video only" },
            { 137, "1080p video only" },
            { 264, "1440p video only" },
            { 266, "2160p video only" },
            { 298, "720p - HFR" },
            { 299, "1080p - HFR" },
            { 304, "1440p - HFR" },
            { 305, "2160p - HFR" },
            { 139, "48Kbps audio only" },
            { 140, "128Kbps audio only" },
            { 141, "256Kbps audio only" },
            { 249, "VBR ~ 50Kbps audio only" },
            { 250, "VBR ~ 70Kbps audio only" },
            { 251, "VBR <= 160Kbps audio only" },
            { 256, "192Kbps - Surround 5.1" },
            { 258, "384Kbps - Surround 5.1" },
            { 327, "256Kbps - Surround 5.1" },
            { 18, "360p - video + audio" },
            { 22, "720p - video + audio" }
        };

        public static long? TotalBytes = 0;
        public static int ReadBytes = 0;
        public static string currentUrl = "";
        public static YouTube youTube = YouTube.Default;
        public static string PathFile = "";
        public static YouTubeVideo? VideoFile;

        public static void DeleteInfo()
        {
            TotalBytes = 0;
            ReadBytes = 0;
            currentUrl = "";
            PathFile = "";
            YouTube youTube = YouTube.Default;
        }
        public async static void PrepareFile(string path, YouTubeVideo video)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new HttpClient();
            using (FileStream outputfile = new FileStream(path, FileMode.Create))
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                using (var request = new HttpRequestMessage(HttpMethod.Head, video.Uri))
                {
                    TotalBytes = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result.Content.Headers.ContentLength;
                }
                using (var input = await client.GetStreamAsync(video.Uri))
                {
                    byte[] buffer = new byte[50 * 1024];
                    int read;
                    while((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await outputfile.WriteAsync(buffer, 0, read);
                        ReadBytes += read;
                    }
                }
                //byte[] buffer = await video.GetBytesAsync();               
                //await outputfile.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
