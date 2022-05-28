using System;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Interfaces
{
	public interface IGenericReopsitories<TEntity> : IDisposable  where TEntity : BaseEntity
	{

	}
}

