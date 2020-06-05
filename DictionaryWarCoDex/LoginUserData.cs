using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWarCoDex
{
     public static class LoginUserData
    {
            private static string usernameLog;
            private static int userIDLog;
            private static string UserPassLog;


            public static string UserNameLOG { get => usernameLog; set => usernameLog = value; }
            public static int UseridLOG { get => userIDLog; set => userIDLog = value; }
            public static string UserPassLOG { get => UserPassLog; set => UserPassLog = value; }


        public static void EmptyLoginData()
        {
            UseridLOG.Equals(null);
            UserNameLOG.Equals(null);
            UserPassLOG.Equals(null);
        }
    }
}
