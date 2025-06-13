using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WarehouseApplication.Dtos;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> Get()
        {
            var documents = await _documentService.GetAllAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDto>> Get(int id)
        {
            var document = await _documentService.GetByIdAsync(id);
            if (document == null) return NotFound();
            return Ok(document);
        }

        [HttpPost]
        public async Task<ActionResult<DocumentDto>> Post(DocumentDto dto)
        {
            var created = await _documentService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DocumentDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var success = await _documentService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }
    }
}
