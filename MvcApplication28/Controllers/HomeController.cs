using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ajax.PeopleData;
using MvcApplication28.Models;

namespace MvcApplication28.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            var viewModel = new IndexViewModel();
            viewModel.People = manager.GetPeople();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddPerson(Person person)
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            manager.AddPerson(person);
            return Json(person);
        }

        public ActionResult GetPeople()
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            return Json(new { people = manager.GetPeople() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            manager.DeletePerson(id);
            return Json(new { status = "OK" });
        }

        public ActionResult GetPerson(int id)
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            return Json(new { person = manager.GetPerson(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePerson(Person person)
        {
            var manager = new PersonManager(Properties.Settings.Default.ConStr);
            manager.UpdatePerson(person);
            return Json(person);
        }


    }
}
