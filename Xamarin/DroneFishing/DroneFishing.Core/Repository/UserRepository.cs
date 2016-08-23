using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneFishing.Core.Model;

namespace DroneFishing.Core.Repository
{
    public class UserRepository
    {
        private static List<User> devices = new List<User>()
        {
            new User()
            {
                UserId = 1,
                UserEmail = "gavin.stevens@gmail.com"
            }
        };

        public List<User> GetAllUsers()
        {
            IEnumerable<User> devs = 
                from device in devices
                select device;

            return devs.ToList<User>();
        }

        public User GetUserById(int deviceId)
        {
            IEnumerable<User> devs =
                from device in devices
                where device.UserId == deviceId
                select device;

            return devs.FirstOrDefault();
        }

    }
}
