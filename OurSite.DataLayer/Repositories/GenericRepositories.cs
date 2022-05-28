using System;
using Microsoft.EntityFrameworkCore;
using OurSite.DataLayer.Contexts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Interfaces;

namespace OurSite.DataLayer.Repositories
{
    public class GenericRepositories <Tentity> : IGenericReopsitories <Tentity> where Tentity : BaseEntity
    {
        
        private readonly DbSet<Tentity> dbset;
        private DataBaseContext context;
        public GenericRepositories(DataBaseContext context)
        {
            this.context = context;
            dbset = context.Set<Tentity>();
        }
        public async Task AddEntity(Tentity entity)
        {
            await dbset.AddAsync(entity);

        }

        public async Task<bool> DeleteEntity(long Id)
        {
            var find = await dbset.AnyAsync(x => x.Id == Id);
            try
            {
                if (find == true)
                {
                    var entity = await dbset.FindAsync(Id);
                    entity.IsRemove = true;
                    UpDateEntity(entity);


                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable GetAllEntity()
        {
            return dbset.AsQueryable();
           
        }

        public async Task<Tentity> GetEntity(long Id)
        {
            var get = await dbset.SingleOrDefaultAsync(x => x.Id == Id);
            return get;
        }

        public async Task SaveEntity()
        {

            await context.SaveChangesAsync();
        }

        public void UpDateEntity(Tentity entity)
        {
            dbset.Update(entity);
            
        }
    }
}

