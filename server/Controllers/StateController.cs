using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using server.Mappers;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _state_repo;
        public StateController(IStateRepository state_repo)
        {
            _state_repo = state_repo;
        }
        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _state_repo.GetAllStatesAsync();
            
            var dtos = states.Select(s=>s.toStateDto());

            return Ok(dtos);
        }
        [Authorize]
        [HttpGet("getById{id}")]
        public async Task<IActionResult> GetStateById([FromRoute] int id)
        {
            var state = await _state_repo.GetStateByIdAsync(id);
            if(state==null)
                return NotFound("State not found");

            return Ok(state.toStateDto());
        }
    }
}