using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFishing.Core.Model
{
    public class User
    {
        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public List<FishingDevice> Devices { get; set; }

    }
}
