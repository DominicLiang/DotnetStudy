using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

//var message = new MimeMessage();
//message.From.Add(new MailboxAddress("Joey Tribbiani", "mytao888@sina.com"));
//message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "dominic1987@foxmail.com"));
//message.Subject = "How you doin'?";

//message.Body = new TextPart("plain")
//{
//    Text = @"Hey Chandler,

//I just wanted to let you know that Monica and I were going to go play some paintball, you in?

//-- Joey"
//};

//using (var client = new SmtpClient())
//{
//    client.Connect("smtp.sina.com", 25, false);

//    // Note: only needed if the SMTP server requires authentication
//    client.Authenticate("mytao888@sina.com", "96bfd8734c7fbf75");

//    var res = client.Send(message);
//    client.Disconnect(true);
//    Console.WriteLine(res.StartsWith("ok"));
//}

//string s = "您好，感谢您在我淘注册账户！\n\n您可以直接点击下方链接进行验证:\n{0}\n\n本邮件无需回复。\n如果不是本人操作，请忽略。\n\n\n--\n我淘";
//Console.WriteLine(string.Format(s, 123));

string msg = "您好，感谢您在我淘注册账户！\n\n您可以直接点击下方链接进行验证:\n{0}\n\n本邮件无需回复。\n如果不是本人操作，请忽略。\n\n\n--\n我淘";
string e = Convert.ToBase64String(Encoding.Default.GetBytes(msg)).Replace("+", "%2B");
string d= Encoding.Default.GetString(Convert.FromBase64String(e.Replace("%2B", "+")));


Console.WriteLine(e);
Console.WriteLine(d);
Console.WriteLine(Convert.ToBase64String(Encoding.Default.GetBytes(msg)));

