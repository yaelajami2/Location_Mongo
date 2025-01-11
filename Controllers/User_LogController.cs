using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Mail;
using System.Net;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User_LogController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MongoDBManager<Log_user> _managQuery;
        public User_LogController(IConfiguration configuration)
        {
            _managQuery = new MongoDBManager<Log_user>(configuration, "Log_user");
        }

        [HttpGet]
        public ActionResult<List<Log_user>> Get()
        {
            var collection = _managQuery.GetCollectionByName<Log_user>();
            return collection.Find(_ => true).ToList();
        }

        [HttpPost("insertById")]
        public async Task<IActionResult> Insert_row([FromBody] Log_user log_User)
        {
            try
            {
                await SendEmailAsync(log_User); // קריאה לשיטה אסינכרונית
                log_User.Id = null; // MongoDB will generate the Id automatically
                await _managQuery.InsertAsync(log_User);
                return CreatedAtAction(nameof(GetById), new { id = log_User.Id }, log_User);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task SendEmailAsync(Log_user log_User)
        {
            try
            {
        
                MailMessage message = new MailMessage(Environment.GetEnvironmentVariable("send_mail"), Environment.GetEnvironmentVariable("to_mail"));


                message.Subject = "צפו באתר שלך";
                message.Body = $"Date: {log_User.Date}, ScreenInfo: {log_User.ScreenInfo}, location: {log_User.location}";
                

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    ;
                    client.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("send_mail"),  Environment.GetEnvironmentVariable("pass_mail"));
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await client.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: {0}", ex.Message);
            }
        }


        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _managQuery.QueryByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Document not found");
                }

                // Return serializable object (e.g., a DTO or BsonDocument)
                return Ok(result.ToJson());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
