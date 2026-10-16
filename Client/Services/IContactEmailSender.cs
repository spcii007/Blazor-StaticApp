using SPCII_APP.Models;

namespace SPCII_APP.Services;

public interface IContactEmailSender
{
    Task SendAsync(ContactFormRequest request,CancellationToken cancellationToken);
}
