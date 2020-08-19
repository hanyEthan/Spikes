using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using domain.DTOs;

namespace infrastructure.Repositories
{
    public class PersonMockRepository
    {
        public IEnumerable<Person> GetAll()
        {
            return new[]
            {
                new Person() { ID = 1, Name = "Jimi Hendrix" },
                new Person() { ID = 2, Name = "James Page" },
                new Person() { ID = 3, Name = "David Gilmour" }
            };
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return GetAll();
        }

        public Person Insert(Person entity)
        {
            return new Person() { ID = new Random(300).Next(), Name = entity.Name };
        }

        public async Task<Person> InsertAsync(Person entity)
        {
            return Insert(entity);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Person GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Person Update(Person entity)
        {
            throw new NotImplementedException();
        }

        public Task<Person> UpdateAsync(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
