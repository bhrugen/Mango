using Mango.EmailAPI.Data;
using Mango.EmailAPI.Models;
using Mango.EmailAPI.Models.Dto;
using Mango.EmailAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        public EmailService(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>" + item.Product.Name+ " - " + item.Count + " x " + item.Product.Price + "</li>");
            }
            message.Append("</ul>");
            await LogAndEmail(message.ToString(), cartDto.Email);
        }


        private async Task<bool> LogAndEmail(string message,string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now
                };

                await using var _db = new ApplicationDbContext(_dbContextOptions);
                await _db.EmailLogger.AddAsync(emailLogger);
                await _db.SaveChangesAsync();
                // You can add your email sending logic here, for example using SMTP or any email service provider API.
                return true;

            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
