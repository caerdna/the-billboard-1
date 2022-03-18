using Microsoft.AspNetCore.Mvc;
using TheBillboard.Abstract;
using TheBillboard.Models;
using TheBillboard.ViewModels;

namespace TheBillboard.Controllers;

public class MessagesController : Controller
{
    private readonly IMessageGateway _messageGateway;
    private readonly IAuthorGateway _authorGateway;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(IMessageGateway messageGateway, ILogger<MessagesController> logger, IAuthorGateway authorGateway)
    {
        _logger = logger;
        _messageGateway = messageGateway;
        _authorGateway = authorGateway;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _messageGateway.GetAll();
        var authors = _authorGateway.GetAll();

        var createViewModel = new MessageCreationViewModel(new Message(), authors);
        var indexModel = new MessagesIndexViewModel(createViewModel, messages);
        return View(indexModel);
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync(int? id)
    {
        if(id is null)
        {
            return View("Error");
        }
        var message = await _messageGateway.GetByIdAsync((int)id);

        if (message is null)
        {
            return View("Error");
        }
        else
        {
            var viewModel = new MessageCreationViewModel(message, _authorGateway.GetAll());
            return View(viewModel);
        }
    }

    [HttpPost]
    public IActionResult Create(Message message)
    {
        if (!ModelState.IsValid)
        {
            return View(new MessageCreationViewModel(message, _authorGateway.GetAll()));
        }

        if (message.Id == default)
        {
            _messageGateway.Create(message);
        }
        else
        {
            _messageGateway.Update(message);
        }

        _logger.LogInformation($"Message received: {message.Title}");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Detail(int id)
    {
        var message = await _messageGateway.GetByIdAsync(id);
        if (message is null)
        {
            return View("Error");
        }
        return View(message);
    }

    public IActionResult Delete(int id)
    {
        _messageGateway.Delete(id);
        return RedirectToAction("Index");
    }

    private MessageWithAuthor MatchAuthorToMessage(Message message, IEnumerable<Author> authors)
        => new MessageWithAuthor(message, authors.FirstOrDefault(a => a.Id == message.AuthorId, new Author("Unknown Author")));
}