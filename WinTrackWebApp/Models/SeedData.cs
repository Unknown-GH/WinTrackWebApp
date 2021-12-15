using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinTrackWebApp.Data;

namespace WinTrackWebApp.Models
{
    public static class SeedData
    {
        private static WintrackContext _winTrackContext;

        public static void Init(WintrackContext wintrackContext)
        {
            _winTrackContext = wintrackContext;
            _winTrackContext.Database.Migrate();
            /*AddUsers();*/
        }

        /*private static void AddUsers()
        {
            var manager = new Applicatino
        }

        private static bool DefaultUserExists() { return _winTrackContext.Users.Any(u => u.UserName == "treindienstleider" && u.Email == "treindienstleider@wintrack.nl"); }*/
    }
}
