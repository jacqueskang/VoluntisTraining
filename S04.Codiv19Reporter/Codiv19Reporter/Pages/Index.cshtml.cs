using Codiv19.Events;
using Codiv19Reporter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Codiv19Reportor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _service;

        public IndexModel(IReportService service)
        {
            _service = service;
        }

        [BindProperty]
        public bool HaveSymptoms { get; set; }

        [BindProperty]
        public bool Fever { get; set; }

        [BindProperty]
        public bool Cough { get; set; }

        [BindProperty]
        public bool Headache { get; set; }

        [BindProperty]
        public bool Others { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HaveSymptoms && !Fever && !Cough && !Headache && !Others)
            {
                ModelState.AddModelError(string.Empty, "Vous devez sélectionner au moin un symptôme.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            ReportSubmitted @event = HaveSymptoms
                ? ReportSubmitted.WithSymptoms(Email, Fever, Cough, Headache, Others)
                : ReportSubmitted.WithoutSymptoms(Email);

            try
            {
                await _service.SubmitReportAsync(@event);
                TempData.SetInfoMessage("Le rapport a été envoyé.");
            }
            catch (Exception ex)
            {
                TempData.SetErrorMessage($"Erreur : {ex.Message}");
            }

            return RedirectToPage();
        }
    }
}
