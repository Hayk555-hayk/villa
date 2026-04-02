using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villa.Data;
using villa.logging;
using villa.Models;
using villa.Models.Dto;

namespace villa.Controllers
{

    [Route("api/getVillas")]
    [ApiController]
    public class VillaController :ControllerBase 
    {

        private readonly ILogger<VillaController> _logger;
        private readonly ILoging _loging;
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ILoging loging, ApplicationDBContext db, IMapper mapper)
        {
            _logger = logger;
            _loging = loging;
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Here are villas");
            _loging.Log("Villa creatin", "Ok");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDto>>(villaList));
        }

        [HttpGet("{id:int}", Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult<VillaDto>> GetVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == id);

            if(villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaCreateDto villaCreateDTO)
        {
            if(villaCreateDTO == null)
            {
                return BadRequest(villaCreateDTO);
            }
            
            if(villaCreateDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if(_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Model Name should be unique");

                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(villaCreateDTO);

            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }        


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            
            if(villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDTO)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            Villa model = _mapper.Map<Villa>(villaUpdateDTO);

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            VillaUpdateDto villaDTO = _mapper.Map<VillaUpdateDto>(villa);

            if(villa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            _db.Update(model);
            _db.SaveChanges();

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
    //https://jsonpatch.com/
}