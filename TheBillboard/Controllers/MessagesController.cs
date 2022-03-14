﻿using Microsoft.AspNetCore.Mvc;
using TheBillboard.Abstract;

namespace TheBillboard.Controllers;

public class MessagesController : Controller
{
    private readonly IMessageGateway _messageGateway;

    public MessagesController(IMessageGateway messageGateway)
    {
        _messageGateway = messageGateway;
    }
    
    public IActionResult Index()
    {
        var messages = _messageGateway.GetMessages();
        return View(messages);
    }
    
    public IActionResult Detail(int id)
    {
        var message = _messageGateway.GetMessage(id);
        return View(message);
    }

    public IActionResult Delete(int id)
    {
        _messageGateway.DeleteMessage(id);
        return RedirectToAction("Index");
    }
}