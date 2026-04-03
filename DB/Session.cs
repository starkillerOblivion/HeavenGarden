using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenGarden.DB
{
    public static class Session
    {
        public static Users CurrentUser { get; set; }

        public static bool IsAuthenticated => CurrentUser != null;

        public static bool IsAdmin => CurrentUser != null && CurrentUser.RoleId == 1;
        public static bool IsGardener => CurrentUser != null && CurrentUser.RoleId == 3;
        public static bool IsRegularUser => CurrentUser != null && CurrentUser.RoleId == 2;

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
