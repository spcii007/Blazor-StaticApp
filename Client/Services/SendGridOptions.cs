namespace SPCII_APP.Services;

public sealed class SmtpSettings
{
    public const string SectionName = "SmtpSetting";
    public string SmtpServer { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromName { get; set; } = "SPCII Technologies Inc.";
    public string ToEmail { get; set; } = "pablo.cifuentes@spcii.com";
    public int Port { get; set; } = 587;
}
