using System.Net;
using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepostiory;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers;
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    // private readonly ILogger<VillaAPIController> _logger;

    protected APIResponse _response;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    public VillaAPIController(IVillaRepository dbVilla, Mapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
        this._response = new();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>>  GetVillas()
    {
        try
        {
            // _logger.LogInformation("Getting All Villas");
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>(){ex.Message};
            _response.StatusCode = HttpStatusCode.NotFound;
            return (_response);
        }
      
    }

    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType( StatusCodes.Status404NotFound)]
    
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {

        try
        {
            if (id == 0)
            {
                // _logger.LogError("Getting Villa with id 0");
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Id is required.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _dbVilla.GetAsync(v => v.Id == id);
            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Villa not found.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>(){ex.Message};
            return (_response);
        }
    }

    [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
    {
        try
        {
            if (await _dbVilla.GetAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Name", "Villa already exists");
                return BadRequest();
            }

            if (villaDTO == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa villa = _mapper.Map<Villa>(villaDTO);

            await _dbVilla.CreateAsync(villa);
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = _mapper.Map<VillaDTO>(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        } catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>(){ex.Message};
            return (_response);
        }
    }

    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        } catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>(){ex.Message};
            return (_response);
        }
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
    {
        try
        {
            if (villaDTO == null || villaDTO.Id != id)
            {
                return BadRequest();
            }

            Villa villa = _mapper.Map<Villa>(villaDTO);

            await _dbVilla.UpdateAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        } catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>(){ex.Message};
            return (_response);
        }
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }
        var villa = await _dbVilla.GetAsync(v => v.Id == id, tracked: false);
        if (villa == null)
        {
            return NotFound();
        }
        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
      
        patchDTO.ApplyTo(villaDTO, ModelState);
        Villa villamodel = _mapper.Map<Villa>(villaDTO);
        
        await _dbVilla.UpdateAsync(villamodel);
       
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}
