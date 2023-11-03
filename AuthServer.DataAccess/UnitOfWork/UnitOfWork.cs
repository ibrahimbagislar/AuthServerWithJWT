using AuthServer.Core.UnitOfWork;
using AuthServer.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly DbContext _context;
        public UnitOfWork(AuthServerAppDbContext context)
        {
            _context = context;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
