using DailyTool.Infrastructure.Abstractions;
using DailyTool.Infrastructure.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Scrummy.Core.BusinessLogic.Data;
using Scrummy.Core.BusinessLogic.Exceptions;

namespace DailyTool.DataAccess.Generic
{
    public class GenericRepository<TModel, TEntity> : IRepository<TModel>
        where TModel : IIdentifiable
        where TEntity : class, IIdentifiableSet
    {
        private readonly IDbContextFactory<ScrummyContext> _dbContextFactory;
        private readonly IMapper<TModel, TEntity> _entityMapper;
        private readonly IMapper<TEntity, TModel> _modelMapper;

        public GenericRepository(
            IDbContextFactory<ScrummyContext> dbContextFactory,
            IMapper<TModel, TEntity> entityMapper,
            IMapper<TEntity, TModel> modelMapper)
        {
            _dbContextFactory = dbContextFactory;
            _entityMapper = entityMapper;
            _modelMapper = modelMapper;
        }

        public async Task<int> CreateAsync(TModel model)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = _entityMapper.Map(model);
            entity.Id = 0;

            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = await dbContext.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (entity is null)
            {
                return;
            }

            dbContext.Set<TEntity>().Remove(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<TModel>> GetAllAsync()
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entitys = dbContext.Set<TEntity>();
            return entitys.Select(_modelMapper.Map).ToList();
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = dbContext.Set<TEntity>().Find(id);
            if (entity is null)
            {
                throw new NotFoundException<TModel>();
            }

            return _modelMapper.Map(entity);
        }

        public async Task UpdateAsync(TModel model)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var entity = dbContext.Set<TEntity>().Find(model.Id);
            if (entity is null)
            {
                throw new NotFoundException<TModel>();
            }

            _entityMapper.Merge(model, entity);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
