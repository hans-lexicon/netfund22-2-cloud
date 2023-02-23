using Microsoft.AspNetCore.Mvc;
using SmartHome.Services;
using SmartHome.ViewModels.Home;

namespace SmartHome.Controllers
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
            var viewModel = new IndexViewModel();
            viewModel.TemperatureMeasurement.CurrentTemperature = 22;


            viewModel.Devices = await _deviceService.GetAllAsync();
            viewModel.DeviceCount = viewModel.Devices.Count();

            return View(viewModel);
        }
    }
}
