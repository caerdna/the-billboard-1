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
        var messages = await _messageGateway.GetAllAsync();
        var authors = await _authorGateway.GetAllAsync();

        var createViewModel = new MessageCreationViewModel(new Message(), authors);
        var indexModel = new MessagesIndexViewModel(createViewModel, messages);
        return View(indexModel);
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync(int? id)
    {
        Message message;

        if (id is null)
        {
            message = new Message();
        }
        else
        {
            message = await _messageGateway.GetByIdAsync((int)id) ?? new Message();
        }

        var viewModel = new MessageCreationViewModel(message, await _authorGateway.GetAllAsync());
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Message message)
    {
        if (!ModelState.IsValid)
        {
            return View(new MessageCreationViewModel(message, await _authorGateway.GetAllAsync()));
        }

        if (message.Id == default)
        {
            await _messageGateway.CreateAsync(message);
        }
        else
        {
            await _messageGateway.UpdateAsync(message);
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

    public async Task<IActionResult> Delete(int id)
    {
        await _messageGateway.DeleteAsync(id);
        return RedirectToAction("Index");
    }

}