using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        Task<TEntity> Repository<TEntity>(); 
        Task<int> Complete();
    }
}