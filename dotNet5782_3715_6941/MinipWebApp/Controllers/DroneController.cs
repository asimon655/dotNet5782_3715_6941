using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PracticalAspNetCore.Controllers
{
    [Route("Drone")]
    public class DroneController : Controller
    {
        private BlApi.Ibl logic;

        public DroneController() => logic = BlApi.BlFactory.GetBl();

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
            
            BO.Drone drone;
            try 
            {
                drone = logic.GetDrone(Id);
            }
            catch (Exception err)
            {
                return RedirectToRoute("Error", err);
            }
            return View(drone);
        }
        [HttpPost]
        [Route("UpdateDrone")]
        public async Task<IActionResult> UpdateDrone(int Id, string Model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.UpdateDrone(Id, Model);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        [HttpPost]
        [Route("BindParcelToDrone")]
        public async Task<IActionResult> BindParcelToDrone(int Id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.BindParcelToDrone(Id);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        [HttpPost]
        [Route("DronePickUp")]
        public async Task<IActionResult> DronePickUp(int Id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.DronePickUp(Id);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        [HttpPost]
        [Route("DroneDelivere")]
        public async Task<IActionResult> DroneDelivere(int Id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.DroneDelivere(Id);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        [HttpPost]
        [Route("DroneCharge")]
        public async Task<IActionResult> DroneCharge(int Id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.DroneCharge(Id);
            }
            catch (Exception err)
            {
                return RedirectToAction("Error", new { err = err });
            }
            return RedirectToAction("Get", new { Id = Id });
        }
        [HttpPost]
        [Route("DroneReleaseCharge")]
        public async Task<IActionResult> DroneReleaseCharge(int Id, double chargingPeriod)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", new { err = new Exception("your request isnt valid") });
            
            try 
            {
                logic.DroneReleaseCharge(Id, chargingPeriod);
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
