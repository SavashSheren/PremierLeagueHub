using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;

namespace PremierLeagueHub.DataAccessLayer.Repositories;

public class GenericRepository<T> : IGenericDal<T> where T : class
{
    private readonly PremierLeagueHubContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(PremierLeagueHubContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}