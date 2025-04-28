using MimeKit;
using Todo_Notifier.Models;

namespace Todo_Notifier.Services;

public class EmailNotifier
{
    public void SendEmail(List<Todo> todos)
    {
        string body = """
                      <html>
                      <head>
                          <style>
                              body { font-family: Arial, sans-serif; }
                              .container { width: auto; margin: auto; }
                              h1 { color: #333; }
                              h2 { color: #555; }
                              table { width: 100%; border-collapse: collapse; margin-top: 20px; text-align: center; }
                              th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                              th { background-color: #f4f4f4; }
                              tr:nth-child(even) { background-color: #f9f9f9; }
                              tr:hover { opacity: 0.8; }
                          </style>
                      </head>
                      <body>
                          <div class='container'>
                      """;


        body += $"<h1>Todos to Complete</h1>";
        body += """
                    <table>
                        <thead>
                            <tr>
                                <th>Title</th>
                            </tr>
                        </thead>
                        <tbody>
                    """;

        body = todos.Aggregate(body, (current, todo) => current + $"<tr><td>{todo.Title}</td></tr>");

        body += "</tbody></table>";

        body += "</div></body></html>";
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Todo Notifier", "asanderson1994s@gmail.com"));
        message.To.Add(new MailboxAddress("Recipient", "asanderson1994s@gmail.com"));
        message.Subject = "Todos to Complete";

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtpClient.Authenticate("asanderson1994s@gmail.com", "xcee rblx eccq rtdj");

        smtpClient.Send(message);
        smtpClient.Disconnect(true);
    }
}
