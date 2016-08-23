using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneFishing.Core.Model;

namespace DroneFishing.Core.Repository
{
    public class FishingDeviceRepository
    {
        private static List<FishingDevice> devices = new List<FishingDevice>()
        {
            new FishingDevice()
            {
                ApiKey = "",
                DeviceId = "3a0044001647353236343033",
                Name = "FishWithChips1",
                UserId = 1
            }
        };

        public List<FishingDevice> GetAllDevices()
        {
            IEnumerable<FishingDevice> devs = 
                from device in devices
                select device;

            return devs.ToList<FishingDevice>();
        }

        public FishingDevice GetDeviceById(string deviceId)
        {
            IEnumerable<FishingDevice> devs =
                from device in devices
                where device.DeviceId == deviceId
                select device;

            return devs.FirstOrDefault();
        }

    }
}
