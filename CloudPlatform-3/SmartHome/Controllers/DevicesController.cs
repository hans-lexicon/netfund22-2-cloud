using Microsoft.AspNetCore.Mvc;
using SmartHome.Services;

namespace SmartHome.Controllers
{
    public class DevicesController : Controller
    {
        private readonly DeviceService _deviceService;

        public DevicesController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public async Task<IActionResult> DirectMethod(string id)
        {
            var result = await _deviceService.SendDirectMethodAsync(id, "IsEnabled");
            return RedirectToAction("Index", "Home");
        }
    }
}
