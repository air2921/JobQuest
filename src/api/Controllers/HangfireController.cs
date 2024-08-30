using api.Utils;
using background;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/hangfire/jobs")]
[ApiController]
[Authorize(Policy = ApiSettings.ADMIN_POLICY)]
public class HangfireController(DeleteExpiredAuth authService, DeleteExpiredRecovery recoveryService) : ControllerBase
{
    [HttpPost("auth")]
    public IActionResult SetRemoveAuthJob()
    {
        RecurringJob.AddOrUpdate(
            "DeleteExpiredAuthJob",
            () => authService.DeleteExpired(),
            Cron.Daily);

        return StatusCode(200);
    }

    [HttpPost("recovery")]
    public IActionResult SetRemoveRecoveryJob()
    {
        RecurringJob.AddOrUpdate(
            "DeleteExpiredRecoveryJob",
            () => recoveryService.DeleteExpired(),
            Cron.Daily);

        return StatusCode(200);
    }
}
