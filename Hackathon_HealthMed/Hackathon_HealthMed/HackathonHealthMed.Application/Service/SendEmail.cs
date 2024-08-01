using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HackathonHealthMed.Application.Service
{
    public class SendEmail : ISendEmail
    {
        private readonly ILoginRepository _login;
        public SendEmail(ILoginRepository login)
        {
            _login = login;
        }
        public async Task Send(long pacienteId, long medicoId, DateTime date)
        {
            try
            {

                var paciente = await _login.GetByIdAsync(pacienteId);
                var medico = await _login.GetByIdAsync(medicoId);

                MailMessage mailMessage = new MailMessage("healthmed95@gmail.com", medico.Email);

                mailMessage.Subject = "Health&Med - Nova consulta agendada";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = @$"<p>Olá, Dr. {medico.Nome}! <br />
                                    <br />
                                    Você tem uma nova consulta marcada! 
                                    Paciente: {paciente.Nome}.
                                    Data e horário: {date:dd/MM/yyyy} às {date:HH:mm}
                                    </p>";


                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("healthmed95@gmail.com", "ypnhyfqddfhjxuuy");
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            

           
        }
    }
}
