using Film.Datas;
using Film.WebAPI.Repository;
using Microsoft.EntityFrameworkCore;


public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet;
    private readonly SampleDBContext _sampleDBContext;

    public Repository(SampleDBContext sampleDBContext)
    {
        _dbSet = sampleDBContext.Set<T>();
        _sampleDBContext = sampleDBContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _sampleDBContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
       _dbSet.Remove(entity);
        await _sampleDBContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _sampleDBContext.Entry(entity).State = EntityState.Modified;
        await _sampleDBContext.SaveChangesAsync();
    }
}
