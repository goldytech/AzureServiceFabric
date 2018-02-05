using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Models;

namespace MyApp.Web.Controllers
{
    using System.Fabric;

    using Microsoft.ServiceFabric.Services.Client;
    using Microsoft.ServiceFabric.Services.Remoting.Client;

    using MyApp.Abstractions;

    public class HomeController : Controller
    {
        private readonly FabricClient fabricClient;

        public HomeController(FabricClient fabricClient)
        {
            this.fabricClient = fabricClient;
        }
        public async Task<IActionResult> Index()
        {
            var serviceUri = new Uri("fabric:/MyApp/MyApp.Counter");
            var model = new Dictionary<Guid, long>();
            foreach (var partition in await this.fabricClient.QueryManager.GetPartitionListAsync(serviceUri).ConfigureAwait(false))
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<ICounterService>(serviceUri, partitionKey);
                var count = await proxy.GetCountAsync().ConfigureAwait(false);
                model.Add(partition.PartitionInformation.Id, count);
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
