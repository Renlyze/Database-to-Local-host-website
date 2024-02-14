using System.Linq;
using System.Web.Mvc;
using CarInsurance.Models;
using CarInsurance.ViewModels;

namespace CarInsurance.Controllers
{
    public class InsureeAdminController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        public ActionResult Index()
        {
            // Retrieve all quotes with user details
            var quotesWithDetails = db.Tables.Select(table => new AdminQuoteViewModel
            {
                Id = table.Id,
                FirstName = table.FirstName,
                LastName = table.LastName,
                EmailAddress = table.EmailAddress,
                Quote = table.Quote
            }).ToList();

            return View(quotesWithDetails);
        }
    }
}
