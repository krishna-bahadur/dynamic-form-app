using System.Diagnostics;
using DynamicForm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace DynamicForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly  AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var forms = _context.DynamicForms.ToList();

            ViewData["FormName"] = forms;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string formName, string formSchema)
        {
            Form form = new Form()
            {
                FormKey = formName,
                FormSchema = formSchema,
            };

            await _context.DynamicForms.AddAsync(form);
            await _context.SaveChangesAsync();

            var forms = _context.DynamicForms.ToList();

            ViewData["FormName"] = forms;

            return View();
        }
       
       
        public async Task<IActionResult> Form(int id)
       {
            var form = _context.DynamicForms.FirstOrDefault(x=>x.Id == id);
            var forms = _context.DynamicForms.ToList();

            ViewData["FormName"] = forms;

            return View(form);
        }

        public async Task<IActionResult> FormReports(int id)
        {
            var forms = _context.DynamicForms.ToList();
            ViewData["FormName"] = forms;

            var formDetails = await _context.FormDetails
                                     .Include(x => x.Form)
                                     .Where(x => x.FormId == id)
                                     .ToListAsync();

            var form = formDetails.FirstOrDefault()?.Form;

            return View(new FormDetailsViewModel
            {
                Form = form,
                FormDetails = formDetails
            });
        }

        [HttpPost]
        public async Task<IActionResult> Form(Dictionary<string, string> formData)
        {
            var submission = new FormDetail();

            if (formData.TryGetValue("formId", out string? formId))
            {
                if (!string.IsNullOrEmpty(formId))
                {
                    _ = int.TryParse(formId, out int form_Id);
                    submission.FormId = form_Id;
                }
            }

            for (int i = 1; i <= 20; i++)
            {
                string fieldName = $"field{i}";
                string propertyName = $"Field{i}";

                if (formData.TryGetValue(fieldName, out string? fieldValue))
                {
                    var property = typeof(FormDetail).GetProperty(propertyName);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(submission, fieldValue);
                    }
                }
            }

            _context.FormDetails.Add(submission);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> FormDetails(int id)
        {
            var formDetails = await _context.FormDetails
                                    .Include(x => x.Form)
                                    .Where(x => x.FormId == 1)
                                    .ToListAsync();
            var form = formDetails.FirstOrDefault()?.Form; 

            return View(new FormDetailsViewModel
            {
                Form = form,
                FormDetails = formDetails
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
