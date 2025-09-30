using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prescription.Models;

namespace Prescription.Controllers
{
    [Route("prescription")]
    public class PrescriptionController : Controller
    {
        private readonly PrescriptionContext _db;
        private readonly string[] _allowedStatuses = new[] { "New", "Filled", "Pending" };

        public PrescriptionController(PrescriptionContext db) => _db = db;

        [HttpGet("add")]
        public IActionResult Add()
        {
            var model = new Prescription.Models.Prescription
            {
                RequestTime = DateTime.Now
            };
            ViewBag.FillStatuses = new SelectList(_allowedStatuses, model.FillStatus);
            return View("Form", model);
        }
        [HttpGet("edit/{id:int}-{slug}")]
        public IActionResult Edit(int id, string? slug)
        {
            var model = _db.Prescriptions.FirstOrDefault(p => p.PrescriptionId == id);
            if (model == null) return RedirectToAction("Index", "Home");

            ViewBag.FillStatuses = new SelectList(_allowedStatuses, model.FillStatus);
            return View("Form", model);
        }
        [HttpPost("save")]
        public IActionResult Save(Prescription.Models.Prescription prescription)
        {
            ViewBag.FillStatuses = new SelectList(_allowedStatuses, prescription.FillStatus);


            if (!_allowedStatuses.Contains(prescription.FillStatus))
            {
                ModelState.AddModelError(nameof(prescription.FillStatus), "Invalid fill status");
            }
            if (prescription.Cost <= 0)
            {
                ModelState.AddModelError(nameof(prescription.Cost), "Cost must be greater than 0");
            }

            if (!ModelState.IsValid)
            {
                return View("Form", prescription);
            }

            if (prescription.PrescriptionId == 0)
            {
                prescription.RequestTime = DateTime.Now;
                _db.Prescriptions.Add(prescription);
                _db.SaveChanges();
            }
            else
            {
                var existing = _db.Prescriptions.FirstOrDefault(p => p.PrescriptionId == prescription.PrescriptionId);
                if (existing == null) return RedirectToAction("Index", "Home");

                existing.MedicationName = prescription.MedicationName;
                existing.FillStatus = prescription.FillStatus;
                existing.Cost = prescription.Cost;
                _db.Update(existing);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("delete/{id:int}-{slug}")]
        public IActionResult Delete(int id, string? slug)
        {
            var model = _db.Prescriptions.FirstOrDefault(p => p.PrescriptionId == id);
            if (model == null) return RedirectToAction("Index", "Home");

            return View("ConfirmDelete", model);
        }
        [HttpPost("deleteconfirmed")]
        public IActionResult DeleteConfirmed(int prescriptionId)
        {
            var entity = _db.Prescriptions.FirstOrDefault(p => p.PrescriptionId == prescriptionId);
            if (entity != null)
            {
                _db.Prescriptions.Remove(entity);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
