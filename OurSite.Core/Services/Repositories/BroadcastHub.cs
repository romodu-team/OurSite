using Microsoft.AspNetCore.SignalR;
using OurSite.Core.Services.Interfaces;

namespace OurSite.Core.Services.Repositories;

public class BroadcastHub:Hub<IHubClientService>
{
    
}
