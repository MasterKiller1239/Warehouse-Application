using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var existing = await _documentService.GetBySymbolAsync(dto.Symbol);
            if (existing != null)
            {
                return Conflict($"A document with symbol '{dto.Symbol}' already exists.");
            }
            try
            {
                if (dto.ContractorId <= 0)
                {
                    return BadRequest("Contractor ID must be provided");
                }

                var created = await _documentService.CreateAsync(dto);

                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DocumentDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var success = await _documentService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }
        [HttpGet("by-symbol/{symbol}")]
        public async Task<ActionResult<DocumentDto>> GetBySymbol(string symbol)
        {
            var document = await _documentService.GetBySymbolAsync(symbol);
            if (document == null)
                return NotFound();

            return Ok(document);
        }
    }
}
