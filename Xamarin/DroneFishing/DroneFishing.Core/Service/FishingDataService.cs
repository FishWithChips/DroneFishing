using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneFishing.Core.Model;
using DroneFishing.Core.Repository;

namespace DroneFishing.Core.Service
{
    public class FishingDataService
    {
        private static FishingDeviceRepository fishingDeviceRepository = new FishingDeviceRepository();
        private static UserRepository userRepository = new UserRepository();

        public List<FishingDevice> GetAllDevices()
        {
            return fishingDeviceRepository.GetAllDevices();
        }

        public FishingDevice GetDeviceById(string deviceId)
        {
            return fishingDeviceRepository.GetDeviceById(deviceId);
        }

        public List<User> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }

    }
}
