using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class locationController : Controller
    {
        private readonly MongoDBManager<location> _managQuery;
        public locationController(IConfiguration configuration)
        {
            _managQuery = new MongoDBManager<location>(configuration, "Location");
        }
        
       
[HttpGet]
        public ActionResult<List<location>> Get()
        {
            var collection = _managQuery.GetCollectionByName<location>();
            return collection.Find(_ => true).ToList();
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


        [HttpDelete("deleteById/{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            try
            {
                var result = await _managQuery.DeleteByIdAsync(id);
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
        [HttpPost("insertById")]
        public async Task<IActionResult> Insert_row([FromBody] location location)
        {
            try
            {
                location.Id = null; // MongoDB will generate the Id automatically
                await _managQuery.InsertAsync(location);
                return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);

               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("updateById/{id}")]
        public async Task<IActionResult> Update_row(string id,[FromBody] location location)
        {
            try
            {
                if (id != location.Id)
                {
                    return BadRequest("ID mismatch");
                }

                await _managQuery.UpdateAsync(id, location);
                return NoContent(); // Return 204 No Content to indicate success
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
