using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Domain.Models.File;
using DataAccess.Repositories;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
            public ActionResult Index([FromServices] FileRepository fileRepository)
        {

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var files = fileRepository.GetFiles().OrderByDescending(f => f.Date).Where(f => f.OwnerId == userId);

                return View(files);

            }

            else
            {
                TempData["error"] = "You need to be connected to view your files";
                return View();
            }
        }

        // GET: FileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FileController/Create
        [HttpGet]
        public ActionResult Add()
        {
            File myModel = new File();
            
            return View(myModel);
        }

        // POST: FileController/Create
        [HttpPost]
        public ActionResult Add(IFormFile f, File file, [FromServices] FileRepository fileRepository)
        {
            if (ModelState.IsValid && (f != null || f.Length != 0))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                file.OwnerId = userId;

                file.Filename= Guid.NewGuid().ToString() + Path.GetExtension(f.FileName);
                fileRepository.AddFile(file);

                TempData["message"] = "File was uploaded successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Check your inputs";


            File myModel = new File();
            myModel = file;

            return View(myModel);
        }

        public IActionResult Download(int id, [FromServices] FileRepository fileRepository)
        {
            var file = fileRepository.GetFiles().FirstOrDefault(f => f.Id == id);

            if (file == null)
            {
                return NotFound();
            }
              
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, file.Filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
                
            var mimeType = "application/octet-stream";

            return PhysicalFile(filePath, mimeType, file.Title + Path.GetExtension(file.Filename));
        }

    }
}
