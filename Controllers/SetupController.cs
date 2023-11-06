using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bfws.Controllers
{
    public class SetupController : Controller
    {
        // GET: /<controller>/
        [Authorize(Roles ="Admin")]
        public ActionResult ListOfCities()
        {
           return RedirectToAction("Index","Cities");
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult ListOfDivisions()
        {
            return RedirectToAction("Index", "Divisions");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListOfEyeColors()
        {
            return RedirectToAction("Index", "EyeColors");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListOfMaritalStates()
        {
            return RedirectToAction("Index", "MaritalStates");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListOfOffices()
        {
            return RedirectToAction("Index", "Offices");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListOfPollingAreas()
        {
            return RedirectToAction("Index", "PollingAreas");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListOfPollingStations()
        {
            return RedirectToAction("Index", "PollingStations");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListOfSkinColors()
        {
            return RedirectToAction("Index", "SkinColors");
        }         
    }
}
