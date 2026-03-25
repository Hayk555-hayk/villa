using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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

        public VillaController(ILogger<VillaController> logger, ILoging loging, ApplicationDBContext db)
        {
            _logger = logger;
            _loging = loging;
            _db = db;
        }


        [HttpGet]
        public ActionResult<IEnumerable<VIllaDTO>> GetVillas()
        {
            _logger.LogInformation("Here are villas");
            _loging.Log("Villa creatin", "Ok");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}", Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VIllaDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VIllaDTO> GetVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u=>u.Id == id);

            if(villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VIllaDTO> CreateVilla([FromBody]VIllaDTO villaDTO)
        {
            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if(_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Model Name should be unique");

                return BadRequest(ModelState);
            }

            Villa model = new Villa()
            {
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl
            };

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
        public IActionResult UpdateVilla(int id, [FromBody] VIllaDTO villaDTO)
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

            Villa model = new Villa()
            {
                Id = id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VIllaDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            VIllaDTO villaDTO = new VIllaDTO()
            {
                Id = villa.Id,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl
            };

            if(villa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new Villa()
            {
                Id = id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl
            };

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