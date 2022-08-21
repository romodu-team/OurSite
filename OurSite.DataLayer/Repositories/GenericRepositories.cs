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
            entity.CreateDate= DateTime.Now;
            entity.LastUpdate= DateTime.Now;
            await dbset.AddAsync(entity);

        }

        public async Task<bool> DeleteEntity(long Id)
        {
            var entity = await dbset.FindAsync(Id);
            try
            {
                if (entity is not null)
                {
                   
                    entity.IsRemove = true;
                    UpDateEntity(entity);
                    return true;

                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IQueryable<Tentity> GetAllEntity()
        {
            return dbset.AsQueryable();
           
        }

        public async Task<Tentity> GetEntity(long Id)
        {
            var get = await dbset.SingleOrDefaultAsync(x => x.Id == Id && x.IsRemove==false);
            return get;
        }

        public async Task<bool> RealDeleteEntity(long Id)
        {

            var entity = await dbset.FindAsync(Id);
            try
            {
                if (entity is not null)
                {
                    
                    dbset.Remove(entity);
                    return true;

                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public Task<Dictionary<string, string>> ReturnLastRecord(Tentity entity)
        {
            
        }

        public Task<Dictionary<string, string>> ReturnLastUpdateRecord(Tentity entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveEntity()
        {

            await context.SaveChangesAsync();
        }

        public void UpDateEntity(Tentity entity)
        {
            entity.LastUpdate= DateTime.Now;
            dbset.Update(entity);

            
        }



        
    }
}

