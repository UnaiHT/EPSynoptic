using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Domain.Models.File;
using DataAccess.Repositories;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using DataAccess.DataContext;
using Presentation.ActionFilters;

namespace Presentation.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }
            // GET: FileController
        public ActionResult Index([FromServices] FileRepository fileRepository, [FromServices] FileDbContext fileDbContext)
        {
            var files = fileRepository.GetFiles().OrderByDescending(f => f.Date);

            var users = fileDbContext.Users
            .ToDictionary(u => u.Id, u => u.UserName);

            var viewModel = files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Title = f.Title,
                OwnerName = users.ContainsKey(f.OwnerId) ? users[f.OwnerId] : "Inconnu",
                Date = f.Date
            }).ToList();

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                File myModel = new File();
            
                return View(myModel);
            }

            else
            {
                TempData["error"] = "You need to be connected to add a file";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Add(IFormFile f, File file, [FromServices] FileRepository fileRepository)
        {
            if (ModelState.IsValid && (f != null || f.Length != 0))
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FileName).ToLowerInvariant();

                var securePath = Path.Combine(_env.ContentRootPath, "PrivateUploads");
                if (!Directory.Exists(securePath))
                {
                    Directory.CreateDirectory(securePath);
                } 

                var filePath = Path.Combine(securePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    f.CopyTo(stream);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                file.OwnerId = userId;

                file.Filename = fileName;
                fileRepository.AddFile(file);

                TempData["message"] = "File was uploaded successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Check your inputs";


            File myModel = new File();
            myModel = file;

            return View(myModel);
        }

        [ServiceFilter(typeof(DownloadActionFilter))]
        public IActionResult Download(int id, [FromServices] FileRepository fileRepository)
        {
            var file = fileRepository.GetFiles().SingleOrDefault(f => f.Id == id);

            if (file == null)
            {
                TempData["error"] = "File not found";
                return RedirectToAction("Index");
            }

            var securePath = Path.Combine(_env.ContentRootPath, "PrivateUploads");
            var filePath = Path.Combine(securePath, file.Filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
                
            var mimeType = "application/octet-stream";

            return PhysicalFile(filePath, mimeType, file.Title + Path.GetExtension(file.Filename));
        }

    }
}
