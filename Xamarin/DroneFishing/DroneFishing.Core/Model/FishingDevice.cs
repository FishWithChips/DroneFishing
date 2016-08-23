using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFishing.Core.Model
{
    public class FishingDevice
    {
        /// <summary>
        /// Id Assigned to chip by Particle
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Name of this device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Access key for device control
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The Device owner's Id
        /// </summary>
        public int UserId { get; set; }

    }
}
