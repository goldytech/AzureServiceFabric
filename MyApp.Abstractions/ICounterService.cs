using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Abstractions
{
    using Microsoft.ServiceFabric.Services.Remoting;

    public interface ICounterService : IService
    {
        Task<long> GetCountAsync();

    }
}
