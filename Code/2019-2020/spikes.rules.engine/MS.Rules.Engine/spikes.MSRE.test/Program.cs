using System;
using System.IO;
using application.Services;
using infrastructure.Repositories;
using spikes.MSRE.core.Framework;

namespace spikes.MSRE.test
{
    class Program
    {
        static void Main()
        {
            try
            {
                var service = new PersonService(new PersonMockRepository(), new MSREValidator());
                var request = File.ReadAllText(@"Data\\P1.json");
                var response = service.InsertCommand(request).GetAwaiter().GetResult();

                if (!response.IsValidRequest) throw new Exception("validation errors.");
                Console.WriteLine("passed.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }
    }
}
