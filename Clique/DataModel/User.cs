using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json;
using Clique.Data;

namespace Clique.DataModel
{
    public class User
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime Joined { get; set; }

        public string Avatar { get; set; }

        public string Reviews { get; set; }

        public string Admin { get; set; }

        public string ID { get; set; }

        public string fontdark { get; set; }

    }

    public class UserSorted
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }

    }

    public class UserDataSource
    {

        public List<UserSorted> Users { get; set; }

        public UserDataSource(string JSON, string username, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            Storage storage = new Storage();

            storage.SaveSettings(sDataKey, JSON);
            storage.SaveSettings(uDataKey, username);
            storage.SaveSettings(pDataKey, password);

        }

    }
}
