using Microsoft.AspNetCore.Mvc;
using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorGateway _authorGateway;

        public AuthorsController(IAuthorGateway authorGateway)
        {
            _authorGateway = authorGateway;
        }

        public IActionResult Index()
        {
            return View(_authorGateway.GetAll());
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _authorGateway.Create(author);

            return RedirectToAction("Index");
        }

        public IActionResult Create(int? id)
        {
            if (id is not null)
            {
                return View(_authorGateway.GetById((int)id));
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            _authorGateway.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
