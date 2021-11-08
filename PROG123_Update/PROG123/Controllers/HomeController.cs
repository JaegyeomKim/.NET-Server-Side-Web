using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PROG123.DAL;
using PROG123.Models;
using PROG123.utils;

namespace PROG123.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // this is for testing purpuse only.
            DatabaseHelper dbh = new DatabaseHelper(_configuration);
            ConnStatusModel status = dbh.GetConnectionStringAndConnectionStatus();
            ViewBag.ConnStr = status.ConnStr;
            ViewBag.DBStatus = status.DBConnectionStatus;
            ViewBag.Exception = status.Exception;
            return View();
        }
        public IActionResult Page2(PersonModel personModel)
        {
            // send the PersonModule to the DB
            //instantiate a DALPerson object
            DALPerson DalPerson = new DALPerson(_configuration);
            //call the DALPerson Addperson to insert perosn module (save the person id)
            string PersonID = DalPerson.AddPerson(personModel);
            personModel.PersonID = PersonID;
            //DALPerson dp = new DALPerson(configuration); 
            // save the personID in to the session  

            HttpContext.Session.SetString("PersonID", PersonID);
            personModel.PersonID = PersonID;


            return View(personModel);
        }


        public IActionResult EditPMyInfor(PersonModel pm)
        {
            //get the UID form the session
            string personID = HttpContext.Session.GetString("PersonID");

            //get the Person object from the DB using DALPerson class 
            DALPerson dp = new DALPerson(_configuration);
            PersonModel person = dp.getPerson(personID);
            //send it to the view.
            return View(person);
        }

        public IActionResult UpdatePersonTable(PersonModel person)
        {
            string personID = HttpContext.Session.GetString("PersonID");
            person.PersonID = personID;
            DALPerson dp = new DALPerson(_configuration);
            dp.UpdatePerson(person);
            return View("Page2", person);
        }
        public IActionResult DeletPerson()
        {
            //get personID form sesstion
            string personID = HttpContext.Session.GetString("PersonID");

            //instaniate and object type  DALPerson
            DALPerson dp = new DALPerson(_configuration);
            //call the dele method from the del person
            dp.DeletePerson(personID);
            return View();
        }


    }
}
