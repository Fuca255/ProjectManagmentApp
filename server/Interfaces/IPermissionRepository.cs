using server.Models;

namespace server.Interfaces
{
    public interface IPermissionRepository
    {
        Task SaveChangesAsync();
    }
}
