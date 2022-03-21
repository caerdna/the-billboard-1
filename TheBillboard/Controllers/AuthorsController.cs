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

        public async Task<IActionResult> Index()
        {
            return View(await _authorGateway.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id is not null)
            {
                return View(await _authorGateway.GetByIdAsync((int)id));
            }

            return View();
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

        public IActionResult Delete(int id)
        {
            _authorGateway.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
