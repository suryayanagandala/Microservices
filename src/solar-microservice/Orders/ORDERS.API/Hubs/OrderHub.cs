using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ORDERS.API.Hubs;

[Authorize]
public sealed class OrderHub : Hub;
