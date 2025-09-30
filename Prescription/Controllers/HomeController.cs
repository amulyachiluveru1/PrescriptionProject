using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prescription.Models;

namespace Prescription.Controllers
{
    public class HomeController : Controller
    {
        private readonly PrescriptionContext _db;
        public HomeController(PrescriptionContext db) => _db = db;

        public IActionResult Index()
        {
            var prescriptions = _db.Prescriptions.OrderBy(p => p.PrescriptionId).ToList();
            return View(prescriptions);
        }
    }
}
