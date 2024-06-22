using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Permissions;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepo;

        public PermissionController(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }
    }
}


