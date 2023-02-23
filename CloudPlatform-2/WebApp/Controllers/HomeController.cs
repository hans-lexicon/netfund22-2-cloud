using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels.Home;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeviceService _deviceService;

        public HomeController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                DevicesFromRegistryManager = await _deviceService.GetDevicesAsync()
            };

            return View(viewModel);
        }
    }
}
