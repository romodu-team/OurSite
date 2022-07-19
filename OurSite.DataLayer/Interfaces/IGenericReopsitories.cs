using System;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Interfaces
{
	public interface IGenericReopsitories<TEntity> : IDisposable  where TEntity : BaseEntity
	{

		Task AddEntity(TEntity entity);
		Task<bool> DeleteEntity(long Id);
		void UpDateEntity(TEntity entity);
		Task SaveEntity();
		IQueryable<TEntity> GetAllEntity();
		Task<TEntity> GetEntity(long Id);

		Task<bool> RealDeleteEntity(long Id);

	}
}

