using Microsoft.AspNetCore.Mvc;
using WarehouseApplication.Dtos;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractorsController : ControllerBase
    {
        private readonly IContractorService _service;

        public ContractorsController(IContractorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractorDto>>> Get()
        {
            var dtos = await _service.GetAllAsync();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContractorDto>> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ContractorDto>> Post(ContractorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ContractorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return BadRequest();

            return NoContent();
        }
        [HttpGet("by-symbol/{symbol}")]
        public async Task<ActionResult<ContractorDto>> GetBySymbol(string symbol)
        {
            var contractor = await _service.GetBySymbolAsync(symbol);
            if (contractor == null)
                return NotFound();

            return Ok(contractor);
        }
    }


}
