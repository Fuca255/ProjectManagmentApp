using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Permissions;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectPermissionController : ControllerBase
    {
        private readonly IUserProjectPermissionRepository _userProjectPermissionRepo;
        private readonly IUserProjectPermissionService _userProjectPermissionService;

        public UserProjectPermissionController(IUserProjectPermissionRepository userProjectPermissionRepo, IUserProjectPermissionService userProjectPermissionService)
        {
            _userProjectPermissionRepo = userProjectPermissionRepo;
            _userProjectPermissionService = userProjectPermissionService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUserProjectPermission([FromBody] UserProjectPermissionDto userProjectPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProjectPermission = UserProjectPermissionMapper.MapToEntity(userProjectPermissionDto);
            await _userProjectPermissionRepo.AddUserProjectPermission(userProjectPermission);

            return Ok(userProjectPermission);
        }
        [HttpPost("add-array")]
        public async Task<IActionResult> AddUserProjectPermissionsArray([FromBody] UserProjectPermissionArrayDto permissionArrayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProjectPermissions = new List<UserProjectPermission>();
            foreach (var dto in permissionArrayDto.Permissions)
            {
                userProjectPermissions.Add(UserProjectPermissionMapper.MapToEntity(dto));
            }

            await _userProjectPermissionRepo.AddUserProjectPermissions(userProjectPermissions);

            return Ok(userProjectPermissions);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemovePermission(int id)
        {
            var permission = await _userProjectPermissionRepo.GetUserProjectPermissionById(id);
            if (permission == null)
            {
                return NotFound();
            }

            await _userProjectPermissionRepo.RemoveUserProjectPermission(permission);
            return NoContent();
        }
    }
}
