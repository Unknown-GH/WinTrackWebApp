using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrackWebApp.Data;

namespace WinTrackWebApp.Models
{
    public static class SeedData
    {
        private static WintrackContext _winTrackContext;
        private static UserManager<IdentityUser> _userManager;

        public static void Init(WintrackContext wintrackContext, UserManager<IdentityUser> userManager)
        {
            _winTrackContext = wintrackContext;
            _userManager = userManager;
            _winTrackContext.Database.Migrate();
            using(var transaction = _winTrackContext.Database.BeginTransaction())
            {
                AddArduino();
                _winTrackContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Arduino ON;");
                _winTrackContext.SaveChanges();
                _winTrackContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Arduino OFF;");
                transaction.Commit();
            }
            AddUsers();

        }

        private static void AddArduino()
        {
            if (!_winTrackContext.Arduino.Any(a => a.Key == 1)) _winTrackContext.Add(new Arduino());
        }

        private static async void AddUsers()
        {
            
            var user = await _userManager.FindByEmailAsync("admin@wintrack.nl");
            if (user == null)
            {
                user = new IdentityUser { UserName = "admin@wintrack.nl", Email = "admin@wintrack.nl" };
                var result = await _userManager.CreateAsync(user, "A3secwintrack6!");
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, code);
                }
            }
        }

        private static bool DefaultUserExists() { return _winTrackContext.Users.Any(u => u.UserName == "treindienstleider" && u.Email == "treindienstleider@wintrack.nl"); }
    }
}
