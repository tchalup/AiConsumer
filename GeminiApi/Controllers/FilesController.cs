using GeminiApi.Models;
using GeminiApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeminiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly GeminiService _geminiService;

        public FilesController(ApiDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        // POST: api/files
        [HttpPost]
        public async Task<ActionResult<Guid>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            var fileRecord = new FileRecord
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType
            };

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileRecord.Content = memoryStream.ToArray();
            }

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();

            return Ok(fileRecord.Id);
        }

        public class AskRequest
        {
            public string Prompt { get; set; } = string.Empty;
        }

        // POST: api/files/{id}/ask
        [HttpPost("{id}/ask")]
        public async Task<IActionResult> Ask(Guid id, [FromBody] AskRequest request)
        {
            var fileRecord = await _context.FileRecords.FindAsync(id);

            if (fileRecord == null)
            {
                return NotFound();
            }

            fileRecord.Prompt = request.Prompt;
            await _context.SaveChangesAsync();

            try
            {
                var geminiResponse = await _geminiService.GenerateContentAsync(request.Prompt, fileRecord.Content, fileRecord.ContentType);
                return Ok(geminiResponse);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception details here
                return StatusCode(500, $"An error occurred while calling the Gemini API: {ex.Message}");
            }
        }
    }
}