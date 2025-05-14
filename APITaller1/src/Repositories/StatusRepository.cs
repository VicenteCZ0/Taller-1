using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITaller1.src.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly StoreContext _context;
    private readonly ILogger<Status> _logger;

    public StatusRepository(StoreContext context, ILogger<Status> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddStatusAsync(Status status)
    {
        await _context.Status.AddAsync(status);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStatusAsync(Status status)
    {
        _context.Status.Remove(status);
        await _context.SaveChangesAsync();
    }

    public async Task<Status> GetStatusByIdAsync(int id)
    {
        return await _context.Status.FindAsync(id)
            ?? throw new Exception("Status not found");
    }

    public async Task<IEnumerable<Status>> GetStatusesAsync()
    {
        return await _context.Status.ToListAsync();
    }

    public async Task UpdateStatusAsync(Status status)
    {
        var existingStatus = await _context.Status.FindAsync(status.StatusID)
            ?? throw new Exception("Status not found");

        existingStatus.StatusName = status.StatusName;

        _context.Status.Update(existingStatus);
        await _context.SaveChangesAsync();
    }
}
