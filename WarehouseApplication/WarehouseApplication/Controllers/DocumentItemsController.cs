using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;

namespace WarehouseApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentItemsController : ControllerBase
    {
        private readonly IWarehouseContext _context;
        private readonly IMapper _mapper;

        public DocumentItemsController(IWarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post(DocumentItemDto dto)
        {
            var entity = _mapper.Map<DocumentItem>(dto);
            _context.DocumentItems.Add(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("by-document/{documentId}")]
        public async Task<ActionResult<IEnumerable<DocumentItemDto>>> GetByDocumentId(int documentId)
        {
            var items = await _context.DocumentItems
                .Where(i => i.DocumentId == documentId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<DocumentItemDto>>(items));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DocumentItemDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = await _context.DocumentItems.FindAsync(id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentItemDto>> Get(int id)
        {
            var item = await _context.DocumentItems.FindAsync(id);
            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<DocumentItemDto>(item));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentItemDto>>> Get()
        {
            // Retrieve all document items from the database
            var items = await _context.DocumentItems.ToListAsync();

            // Map the entities to DTOs
            var dtos = _mapper.Map<IEnumerable<DocumentItemDto>>(items);

            // Return the list of DTOs
            return Ok(dtos);
        }
    }


}
