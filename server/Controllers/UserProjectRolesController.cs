using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Permissions;
using server.Mappers;
using server.Repositories;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectRolesController : ControllerBase
    {
        private readonly IUserProjectRolesRepository _userProjectRolesRepository;

        public UserProjectRolesController(IUserProjectRolesRepository userProjectRolesRepository)
        {
            _userProjectRolesRepository = userProjectRolesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProjectRolesDto>>> GetUserProjectRoles()
        {
            var userProjectRoles = await _userProjectRolesRepository.GetAllUserProjectRoles();
            var userProjectRolesDto = userProjectRoles.Select(UserProjectRolesMapper.MapToDto);
            return Ok(userProjectRolesDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserProjectRolesDto>> AddUserProjectRoles(UserProjectRolesDto userProjectRolesDto)
        {
            var userProjectRoles = UserProjectRolesMapper.MapToEntity(userProjectRolesDto);
            await _userProjectRolesRepository.AddUserProjectRoles(userProjectRoles);

            // Return the created user project roles DTO along with the URI of the resource
            return CreatedAtAction(nameof(GetUserProjectRoles), new { id = userProjectRoles.Id }, userProjectRolesDto);
        }
    }
}
