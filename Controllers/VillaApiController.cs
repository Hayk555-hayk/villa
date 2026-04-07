using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villa.Data;
using villa.logging;
using villa.Models;
using villa.Models.Dto;
using villa.Repository.IRepository;

namespace villa.Controllers
{

    [Route("api/getVillas")]
    [ApiController]
    public class VillaController :ControllerBase 
    {

        private readonly ILogger<VillaController> _logger;
        private readonly ILoging _loging;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ILoging loging, IVillaRepository db, IMapper mapper)
        {
            _logger = logger;
            _loging = loging;
            _dbVilla = db;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Here are villas");
            _loging.Log("Villa creatin", "Ok");
            IEnumerable<Villa> villaList = await _dbVilla.GetAll();
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

            var villa = await _dbVilla.Get(u => u.Id == id, false);

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
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto villaCreateDTO)
        {
            if(villaCreateDTO == null)
            {
                return BadRequest(villaCreateDTO);
            }
            
            if(villaCreateDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if(await _dbVilla.Get(u => u.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Model Name should be unique");

                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(villaCreateDTO);

            await _dbVilla.Create(model);

            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }        


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.Get(u => u.Id == id, false);
            
            if(villa == null)
            {
                return NotFound();
            }

            await _dbVilla.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDTO)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.Get(u => u.Id == id, false);

            if (villa == null)
            {
                return NotFound();
            }

            Villa model = _mapper.Map<Villa>(villaUpdateDTO);
            model.Id = id; // Ensure the ID is set

            await _dbVilla.Create(model); // Actually update, not create - wait, this should be update logic
            // Actually, we need to update the existing entity
            _mapper.Map(villaUpdateDTO, villa);
            await _dbVilla.Save();

            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.Get(u => u.Id == id, false);

            if(villa == null)
            {
                return BadRequest();
            }

            VillaUpdateDto villaDTO = _mapper.Map<VillaUpdateDto>(villa);

            patchDTO.ApplyTo(villaDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(villaDTO, villa);
            await _dbVilla.Save();

            return NoContent();
        }
    }
    //https://jsonpatch.com/
}