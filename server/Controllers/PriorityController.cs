using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Mappers;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityRepository _prio_repo;
        public PriorityController(IPriorityRepository prio_repo)
        {
            _prio_repo = prio_repo;
        }
        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPriorities()
        {
            var prios = await _prio_repo.GetPriorities();
            
            var dtos = prios.Select(s=>s.toPrioDto());

            return Ok(dtos);
        }
        [Authorize]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetPrioById([FromRoute] int id)
        {
            var prio = await _prio_repo.GetPriority(id);
            if(prio==null)
                return NotFound("Priority not found");

            return Ok(prio.toPrioDto());
        }
    }
        
}