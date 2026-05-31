using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentTeacherAPI.DAL;
using StudentTeacherAPI.Models;
using System.Security.Claims;

namespace StudentTeacherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require login
    public class RecordsController : ControllerBase
    {
        private readonly RecordDAL _recordDAL;

        public RecordsController(IConfiguration config)
        {
            _recordDAL = new RecordDAL(config.GetConnectionString("DefaultConnection")!);
        }

        // GET: api/records — Both Student and Teacher can access
        [HttpGet]
        public IActionResult GetAll()
        {
            var records = _recordDAL.GetAllRecords();
            return Ok(records);
        }

        // GET: api/records/5 — Both Student and Teacher can access
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var record = _recordDAL.GetRecordById(id);
            if (record == null)
                return NotFound("Record not found.");
            return Ok(record);
        }

        // POST: api/records — Teacher only
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public IActionResult Create([FromBody] Record record)
        {
            // Get logged in user's ID from JWT token
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            record.CreatedBy = 1; // placeholder, we'll use email to find user if needed
            _recordDAL.CreateRecord(record);
            return Ok("Record created successfully!");
        }

        // PUT: api/records/5 — Teacher only
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public IActionResult Update(int id, [FromBody] Record record)
        {
            var existing = _recordDAL.GetRecordById(id);
            if (existing == null)
                return NotFound("Record not found.");

            record.RecordId = id;
            _recordDAL.UpdateRecord(record);
            return Ok("Record updated successfully!");
        }

        // DELETE: api/records/5 — Teacher only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(int id)
        {
            var existing = _recordDAL.GetRecordById(id);
            if (existing == null)
                return NotFound("Record not found.");

            _recordDAL.DeleteRecord(id);
            return Ok("Record deleted successfully!");
        }
    }
}