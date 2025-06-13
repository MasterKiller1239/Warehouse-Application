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
    public class ContractorsController : ControllerBase
    {
        private readonly WarehouseContext _context;
        private readonly IMapper _mapper;

        public ContractorsController(WarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractorDto>>> Get() =>
            Ok(_mapper.Map<IEnumerable<ContractorDto>>(await _context.Contractors.ToListAsync()));

        [HttpPost]
        public async Task<ActionResult> Post(ContractorDto dto)
        {
            var entity = _mapper.Map<Contractor>(dto);
            _context.Contractors.Add(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ContractorDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = await _context.Contractors.FindAsync(id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
