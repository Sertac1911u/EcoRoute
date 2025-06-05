using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EcoRoute.Notifications.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            var user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                var roles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

                foreach (var role in roles)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Role_" + role);
                }
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, "AllUsers");
        }

        public async Task LeaveGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);

            var user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                var roles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

                foreach (var role in roles)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Role_" + role);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AllUsers");
        }
    }

}
