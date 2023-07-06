using APITracker.Repositories;
using APITracker_Service.Data;
using APITracker_Service.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APITracker_Service.Repositories;
public interface IEnderecoApiRepository : IBaseRepositorio
{
    Task Alterar(EnderecoAPI entidade);
    Task<IEnumerable<EnderecoAPI>> BuscarTodos();
}
public class EnderecoApiRepository : BaseRepositorio, IEnderecoApiRepository
{
    private readonly DbSet<EnderecoAPI> _dbSet;

    public EnderecoApiRepository(BaseContext context) : base(context)
    {
        _dbSet = context.Set<EnderecoAPI>();
    }

    public async Task Alterar(EnderecoAPI entidade)
    {
        await IniciarTransaction();
        _context.Entry(entidade).State = EntityState.Modified;
        await SalvarMudancas();
    }

    public async Task<IEnumerable<EnderecoAPI>> BuscarTodos()
    {
        return await Buscar(x => true).ToListAsync();
    }

    public IQueryable<EnderecoAPI> Buscar(Expression<Func<EnderecoAPI, bool>> expression)
    {
        return _dbSet.Where(expression).AsNoTracking();
    }
}
    




