using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

await Execute();

static async Task Execute()
{
    var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
    var client = new SendGridClient(apiKey);
    var from = new EmailAddress("dominic1987@foxmail.com", "Example User");
    var subject = "Sending with SendGrid is Fun";
    var to = new EmailAddress("290707804@qq.com", "Example User");
    var plainTextContent = "and easy to do anywhere, even with C#";
    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
    var response = await client.SendEmailAsync(msg);
    await Console.Out.WriteLineAsync(response.IsSuccessStatusCode.ToString());
    await Console.Out.WriteLineAsync(response.Headers.ToString());
    await Console.Out.WriteLineAsync(response.Body.ToString());
}