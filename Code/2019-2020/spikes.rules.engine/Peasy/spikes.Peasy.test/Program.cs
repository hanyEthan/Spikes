using System;
using spikes.Peasy.core.DTOs;
using spikes.Peasy.core.Repositories;
using spikes.Peasy.core.Services;

namespace spikes.Peasy.test
{
    class Program
    {
        static void Main()
        {
            var service = new PersonService(new PersonMockRepository());
            var newPerson = new Person() { Name = "Freida Jones" };
            var insertResult = service.InsertCommand(newPerson).Execute();

            if (insertResult.Success)
            {
                Console.WriteLine(insertResult.Value.ID.ToString());
            }
            else
            {
                foreach (var error in insertResult.Errors)
                    Console.WriteLine(error);
            }
        }
    }
}
