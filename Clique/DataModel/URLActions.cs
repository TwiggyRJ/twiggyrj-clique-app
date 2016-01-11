using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clique.DataModel
{
    class URLActions
    {

    }
    public class RegisterUser
    {
        public RegisterUser(string name, string action)
        {
            Name = name;
            Action = action;
        }

        public string Name;
        public string Action;
    }
}
