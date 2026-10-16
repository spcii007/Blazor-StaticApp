namespace SPCII_APP.Models;

public sealed record ContactFormRequest(
    string Name,
    string Email,
    string Company,
    string Message);
