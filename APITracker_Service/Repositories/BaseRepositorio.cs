using APITracker_Service.Data;
using System.Threading.Tasks;

namespace APITracker.Repositories
{
    public interface IBaseRepositorio
    {
        Task IniciarTransaction();
        Task SalvarMudancas(bool commit = true);
        Task RollbackTransaction();
        Task Commit();
    }

    public class BaseRepositorio : IBaseRepositorio
    {
        protected readonly BaseContext _context;

        public BaseRepositorio(BaseContext context)
        {
            _context = context;
        }

        public async Task IniciarTransaction()
        {
            await _context.IniciarTransaction();
        }

        public async Task SalvarMudancas(bool commit = true)
        {
            await _context.SalvarMudancas(commit);
        }

        public async Task RollbackTransaction()
        {
            await _context.RollBack();
        }

        public async Task Commit()
        {
            await _context.Commit();
        }
    }
}
