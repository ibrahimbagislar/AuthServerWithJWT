
using SharedLibrary.DTOs;
using System.Linq.Expressions;

namespace AuthServer.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {

        Task<Response<TDto>> GetByIdAsync(object id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
        Task<Response<TDto>> AddAsync(TDto dto);
        Task<Response<NoDataDto>> UpdateAsync(TDto dto, object id);
        Task<Response<NoDataDto>> RemoveAsync(object id);
    }
}
