using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PROG123.DAL;
using PROG123.Models;

namespace PROG123.Controllers
{
    public class KimController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        static string session;


        public KimController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(LogInCredentialsModel logInCredentialsModel)
        {
            //creat a DELperson objeck
            DALPerson DalPerson = new DALPerson(_configuration);
            //user the DELPerson.check loginCredintials Method to check if the crdentials are vaild
            PersonModel person = DalPerson.CheckLogInCredentials(logInCredentialsModel);

            if (person == null)
            {
                ViewBag.LoginMessage = "Sorry. Login Fail. Try again.";
            }
            else
            {
                //PersonModel person = DalPerson.CheckLogInCredentials(logInCredentialsModel);
                ViewBag.UserFirstName = person.FName;
                string personID = person.PersonID;
                session = personID;

            }

            return View("Index");
        }


        public IActionResult EnterNewProduct()
        {

            if (session == null)
            {
                ViewBag.LoginMessage = "User is not logged in";
                return View("Index");
            } 
            else
            {
                return View();
            }


        }


        public IActionResult AddProductToDB(ProductModel productModel)
        {
            if (session == null)
            {
                ViewBag.LoginMessage = "User is not logged in";
                return View("Index");
            }
            else
            {
                DALProducts DalProducts = new DALProducts(_configuration);
                string productID = DalProducts.AddNewProduc(productModel);
                productModel.PID = productID;
                return View("AddProductToDB", productModel);
            }

        }

        public IActionResult ListAllProducts()
        {
            if (session == null)
            {
                ViewBag.LoginMessage = "User is not logged in";
                return View("Index");
            }
            else
            {                
                DALProducts DalProducts = new DALProducts(_configuration);

                LinkedList<ProductModel> list = new LinkedList<ProductModel>();

                list = DalProducts.GetAllProducts();

                return View(list);
            }
        }

        public IActionResult OneClickBuy(string PID)
        {
            if (session == null)
            {
                ViewBag.LoginMessage = "User is not logged in";
                return View("Index");
            }
            else
            {

                string PersonID = session;
                DALSalesTransaction dALSalesTransaction = new DALSalesTransaction(_configuration);

                //(string productID, string personID, int manyPurchasedProds)
                dALSalesTransaction.OneClickBuy(PID, PersonID, 1);

                PurchaseModel purchaseModel = dALSalesTransaction.OneClickBuy(PID, PersonID, 1);

                return View(purchaseModel);
            }
        }

    }
}
