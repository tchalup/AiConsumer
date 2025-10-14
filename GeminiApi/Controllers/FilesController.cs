using GeminiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeminiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FilesController(ApiDbContext context)
        {
            _context = context;
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

            // Aqui você enviaria o arquivo (fileRecord.Content) e o prompt (request.Prompt)
            // para a API do Gemini. Como isso é uma simulação, vamos apenas retornar uma
            // mensagem de sucesso.

            var geminiResponse = $"Pergunta recebida para o arquivo '{fileRecord.FileName}' com o prompt: '{request.Prompt}'. O conteúdo do arquivo seria enviado ao Gemini.";

            return Ok(geminiResponse);
        }
    }
}