using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;

namespace WarehouseApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly WarehouseContext _context;
        private readonly IMapper _mapper;

        public DocumentsController(WarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> Get() =>
            Ok(_mapper.Map<IEnumerable<DocumentDto>>(await _context.Documents
                .Include(d => d.Items)
                .ToListAsync()));

        [HttpPost]
        public async Task<ActionResult> Post(DocumentDto dto)
        {
            var entity = _mapper.Map<Document>(dto);
            _context.Documents.Add(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DocumentDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = await _context.Documents.Include(d => d.Items).FirstOrDefaultAsync(d => d.Id == id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
