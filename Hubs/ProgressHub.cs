using Microsoft.AspNetCore.SignalR;
using YTC.src;

namespace YTC.Hubs
{
    public class ProgressHub : Hub
    {
        public async Task Send()
        {
            int readbytes = StaticData.ReadBytes;
            await Clients.All.SendAsync("Receive", readbytes);
        }
    }
}
