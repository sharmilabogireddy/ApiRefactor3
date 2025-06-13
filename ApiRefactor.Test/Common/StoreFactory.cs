using ApiRefactor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRefactor.Test.Common
{
    public static class StoreFactory
    {
        public static List<Wave> WaveStore = new()
        {
            new Wave
            {
                Id = Guid.Parse("16227e66-bc51-4b47-8b0d-86027b0319c5"),
                Name = "Test1",
                WaveDate = DateTime.Parse("2025-06-13T04:09:18.9071442")
            },
            new Wave
            {
                Id = Guid.Parse("cbfac2cd-5665-45e2-9280-923c97c95630"),
                Name = "Test2",
                WaveDate = DateTime.Parse("2025-06-13T04:07:44.6128966")
            },
        };
    }
}
