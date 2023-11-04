using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using System.Linq.Expressions;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<TEntity> _repository;
        public GenericService(IUnitOfWork uow, IGenericRepository<TEntity> repository)
        {
            _uow = uow;
            _repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            await _repository.AddAsync(newEntity);
            await _uow.SaveChangesAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var data = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await _repository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(data, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            var data = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await _repository.GetByFilter(filter).ToListAsync());
            if (data == null)
                return Response<IEnumerable<TDto>>.Fail("Data Not Found",404);
            return Response<IEnumerable<TDto>>.Success(data, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(object id)
        {
            var data = ObjectMapper.Mapper.Map<TDto>(await _repository.GetByIdAsync(id));
            if (data == null)
                return Response<TDto>.Fail("Data Not Found",404);
            return Response<TDto>.Success(data, 200);
        }

        public async Task<Response<NoDataDto>> RemoveAsync(object id)
        {
            var removedEntity = await _repository.GetByIdAsync(id);
            if (removedEntity == null)
                return Response<NoDataDto>.Fail("Data Not Found",404); 

            _repository.Remove(removedEntity);
            await _uow.SaveChangesAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto entity,object id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);
            if (isExistEntity == null)
                return Response<NoDataDto>.Fail("Data Not Found",404);

            _repository.Update(ObjectMapper.Mapper.Map<TEntity>(entity));
            await _uow.SaveChangesAsync();
            return Response<NoDataDto>.Success(204);
        }
    }
}
