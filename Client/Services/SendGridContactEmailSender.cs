using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SPCII_APP.Models;

namespace SPCII_APP.Services;

public sealed class SendGridContactEmailSender : IContactEmailSender
{
   
    private readonly SmtpSettings _options;
    private readonly MimeMessage message = new();

    public SendGridContactEmailSender(IOptions<SmtpSettings> options)
    {
       
        _options = options.Value;
    }

    public async Task SendAsync(ContactFormRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.SmtpServer) || string.IsNullOrWhiteSpace(_options.Username) 
                                   || _options.Port == 0 || string.IsNullOrWhiteSpace(_options.Password))
        {
            throw new InvalidOperationException("Email server configuration is incomplete.");
        }

        message.From.Add(new MailboxAddress(_options.FromName, _options.Username));
        message.To.Add(new MailboxAddress("Admin", _options.ToEmail));
        message.Subject = $"SPCII Technologies Inc. request from {request.Name}";

        message.Body = new TextPart("plain")
        {
            Text = BuildMessageBody(request)
        };
        try
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_options.SmtpServer, _options.Port, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_options.Username, _options.Password);

                await client.SendAsync(message, cancellationToken);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send email. Please check the SMTP server configuration.", ex);
        }
   }

    private static string BuildMessageBody(ContactFormRequest request)
    {
        var company = string.IsNullOrWhiteSpace(request.Company) ? "Not provided" : request.Company;

        return $"""
        New website contact submission.

        Name: {request.Name}
        Email: {request.Email}
        Company: {company}

        Message:
        {request.Message}
        """;
    }
}
