using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PracticalAspNetCore.Controllers
{
    [Route("Station")]
    public class StationController : Controller
    {
        private BlApi.Ibl logic;

        public StationController() => logic = BlApi.BlFactory.GetBl();

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // [HttpPost]
        [Route("Get")]
        public async Task<IActionResult> Get(int Id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            BO.Station station;
            try 
            {
                station = logic.GetStation(Id);
            }
            catch (Exception err)
            {
                return RedirectToRoute("Error", err);
            }
            return View(station);
        }
        [HttpPost]
        [Route("UpdateStation")]
        public async Task<IActionResult> UpdateStation(int Id, int? stationName = null, int? stationChargeSlots = null)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.UpdateStation(Id, stationName, stationChargeSlots);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        // [HttpPost]
        [Route("Error")]
        public async Task<IActionResult> Error(Exception err)
        {
            Console.WriteLine(err);
            return View(err);
        }
    }
}
