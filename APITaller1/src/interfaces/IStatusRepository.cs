using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IStatusRepository
    {
        Task AddStatusAsync(Status status);
        Task DeleteStatusAsync(Status status);
        Task<Status> GetStatusByIdAsync(int id);
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task UpdateStatusAsync(Status status);
    }
}
