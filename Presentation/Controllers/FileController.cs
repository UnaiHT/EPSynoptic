using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Domain.Models.File;
using DataAccess.Repositories;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class FileController : Controller
    {
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
        public ActionResult Add(File file, [FromServices] FileRepository fileRepository)
        {
            if (ModelState.IsValid)
            {
                fileRepository.AddFile(file);
                TempData["message"] = "File was uploaded successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Check your inputs";


            File myModel = new File();
            myModel = file;

            return View(myModel);
        }

        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
