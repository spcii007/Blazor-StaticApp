using Microsoft.AspNetCore.Mvc;
using SPCII_APP.Models;
using SPCII_APP.Services;

namespace SPCII_APP.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContactController : ControllerBase
{
    private readonly IContactEmailSender _contactEmailSender;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactEmailSender contactEmailSender, ILogger<ContactController> logger)
    {
        _contactEmailSender = contactEmailSender;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Send(
        [FromForm] string name,
        [FromForm] string email,
        [FromForm] string? company,
        [FromForm] string message,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            return Redirect("/?contactStatus=invalid#contact");
        }

        try
        {
            await _contactEmailSender.SendAsync(new ContactFormRequest(
                name.Trim(),
                email.Trim(),
                (company ?? string.Empty).Trim(),
                message.Trim()), cancellationToken);

            return Redirect("/?contactStatus=sent#contact");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact form email.");
            return Redirect("/?contactStatus=error#contact");
        }
    }
}