using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UAutoServiceLearn.Models;

namespace UAutoServiceLearn.ViewModels
{
    public class CarAndServicesViewModel
    {
        public int CarId { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Style { get; set; }
        public int Year { get; set; }

        public string UserId { get; set; }

        public Service NewService { get; set; }

        public List<ServiceType> LstServiceType { get; set; }

        public IEnumerable<Service> PastServices { get; set; }
    }
}
