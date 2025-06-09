using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Presentation.ActionFilters
{
    public class DownloadActionFilter : ActionFilterAttribute
    {

        private readonly FileRepository _fileRepository;

        public DownloadActionFilter(FileRepository fileRepository)
        {
            this._fileRepository = fileRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            /*if (context.ActionArguments.ContainsKey("id") && (int)context.ActionArguments["id"] == 1)
            {
                context.Result = new BadRequestObjectResult("L'appel est interdit si l'ID est 0.");
            }*/

            var user = context.HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var tempDataFactory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(context.HttpContext);

            var file = _fileRepository.GetFiles().SingleOrDefault(f => f.Id == (int)context.ActionArguments["id"]);

            if (userId == null)
            {
                tempData["error"] = "You need to be logged in!";
                context.Result = new RedirectToActionResult("Index", "File", null);

                return;
            }
            else if (userId != file.OwnerId)
            {
                tempData["error"] = "Only the owner can download this file !";
                context.Result = new RedirectToActionResult("Index", "File", null);

                return;
            }

            
        }

    }
}
