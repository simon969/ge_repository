using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ge_repository.services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
 /*   public class xAuthMessageSender : IEmailSender, ISmsSender
    {
       
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
                try  
                {  
                    //From Address    
                    string FromAddress = "myname@company.com";  
                    string FromAdressTitle = "My Name";  
                    //To Address    
                    string ToAddress = email;  
                    string ToAdressTitle = "Microsoft ASP.NET Core";  
                    string Subject = subject;  
                    string BodyContent = message;  
      
                    //Smtp Server    
                    string SmtpServer = "smtp.office365.com";  
                    //Smtp Port Number    
                    int SmtpPortNumber = 587;  
      
                    var mimeMessage = new MimeMessage();  
                    mimeMessage.From.Add(new MailboxAddress  
                                            (FromAdressTitle,   
                                             FromAddress  
                                             ));  
                    mimeMessage.To.Add(new MailboxAddress  
                                             (ToAdressTitle,  
                                             ToAddress  
                                             ));  
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")  
                    {  
                        Text = BodyContent  
                    };  
      
                    using (var client = new SmtpClient())  
                    {  
                        client.Connect(SmtpServer, SmtpPortNumber, false);  
                        client.Authenticate(  
                            "myname@company.com",   
                            "MYPassword"  
                            );  
                 //      await client.SendAsync(mimeMessage);  
                        Console.WriteLine("The mail has been sent successfully !!");  
                        Console.ReadLine();  
                 //      await client.DisconnectAsync(true);  
                    }  
                }  
                catch (Exception ex)  
                {  
                    throw ex;  
                }   

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    } */
}
