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
            return View(await _authorGateway.GetAllAsync().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            Author author;

            if (id is null)
            {
                author = new Author();
            }
            else
            {
                author = await _authorGateway.GetByIdAsync((int)id) ?? new Author();
            }

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            if (author.Id is null)
            {
                await _authorGateway.CreateAsync(author);
            }
            else
            {
                await _authorGateway.UpdateAsync(author);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var author = await _authorGateway.GetByIdAsync(id);
            if (author is null)
            {
                return View("Error");
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _authorGateway.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
