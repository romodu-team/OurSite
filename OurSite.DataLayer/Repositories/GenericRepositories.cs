using System;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Interfaces;

namespace OurSite.DataLayer.Repositories
{
    public class GenericRepositories<Tentity> : IGenericReopsitories<Tentity> where Tentity : BaseEntity
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

