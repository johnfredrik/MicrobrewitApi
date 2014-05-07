using Microbrewit.Model;
using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Test
{
    public class OtherTestRepository : IOtherRepository
    {
        public IList<Other> GetAll(params string[] navigationProperties)
        {
            var allOthers = new List<Other>()
            {
                new Other() { Id = 1, Name = "Strawberry", Type = "Fruit" },
                new Other() { Id = 2, Name = "Honey", Type= "NoneFermentableSugar" },
                new Other() { Id = 3, Name = "Koriander", Type = "Spice" },
            };

            return allOthers;
        }

        public IList<Other> GetList(Func<Other, bool> where, params string[] navigationProperties)
        {
            throw new NotImplementedException();
        }

    
        public void Add(params Other[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params Other[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params Other[] items)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Other>> GetAllAsync(params string[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Task<Other> GetSingleAsync(System.Linq.Expressions.Expression<Func<Other, bool>> where, params string[] navigtionProperties)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(params Other[] items)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(params Other[] items)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(params Other[] items)
        {
            throw new NotImplementedException();
        }


        public IList<Other> GetList(System.Linq.Expressions.Expression<Func<Other, bool>> where, params string[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Other GetSingle(Func<Other, bool> where, params string[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
