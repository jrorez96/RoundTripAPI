using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace RoundTripAPI.Controllers
{
    public class EmailServiceController
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Cambia esto según tu servidor SMTP
        private readonly int _smtpPort = 587; // Puerto SMTP para Gmail
        private readonly string _smtpUser = "tu-correo@gmail.com"; // Tu correo
        private readonly string _smtpPass = "tu-contraseña"; // Tu contraseña

        public async Task SendConfirmationEmailAsync(RentalData rentalData)
        {
            var clientEmail = rentalData.ClientEmail; // Email del cliente, debe estar en los datos de la renta
            var adminEmail = "admin@rentacar.com"; // Email del administrador

            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                smtpClient.EnableSsl = true;

                // Correo para el cliente
                var mailMessageClient = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = "Confirmación de Renta de Vehículo",
                    Body = $"Estimado {rentalData.Name},\n\n" +
                           "Gracias por su reserva. Aquí están los detalles de su renta:\n\n" +
                           $"Vehículo: {rentalData.Marca} {rentalData.Modelo}\n" +
                           $"Fecha de Recogida: {rentalData.FechaRecogida}\n" +
                           $"Fecha de Entrega: {rentalData.FechaEntrega}\n" +
                           $"Lugar de Recogida: {rentalData.LugarRecogida}\n" +
                           $"Lugar de Entrega: {rentalData.LugarEntrega}\n" +
                           $"Costo Total: ${rentalData.TotalCost}\n\n" +
                           "¡Gracias por elegirnos!\n\nAtentamente,\nEquipo de RoundTrip",
                    IsBodyHtml = false
                };
                mailMessageClient.To.Add(clientEmail);

                // Correo para el administrador
                var mailMessageAdmin = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = "Nueva Reserva de Vehículo",
                    Body = $"Se ha realizado una nueva reserva. Detalles:\n\n" +
                           $"Nombre del Cliente: {rentalData.Name} {rentalData.Lastname}\n" +
                           $"Pasaporte: {rentalData.Passport}\n" +
                           $"Vehículo: {rentalData.Marca} {rentalData.Modelo}\n" +
                           $"Fecha de Recogida: {rentalData.FechaRecogida}\n" +
                           $"Fecha de Entrega: {rentalData.FechaEntrega}\n" +
                           $"Lugar de Recogida: {rentalData.LugarRecogida}\n" +
                           $"Lugar de Entrega: {rentalData.LugarEntrega}\n" +
                           $"Costo Total: ${rentalData.TotalCost}\n\n" +
                           "Revise el sistema para más detalles.",
                    IsBodyHtml = false
                };
                mailMessageAdmin.To.Add(adminEmail);

                // Enviar correos
                await smtpClient.SendMailAsync(mailMessageClient);
                await smtpClient.SendMailAsync(mailMessageAdmin);
            }
        }
    }
}
