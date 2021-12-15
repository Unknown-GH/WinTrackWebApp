using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinTrackWebApp.Data
{
    public class WintrackContext : IdentityDbContext
    {
        public WintrackContext(DbContextOptions<WintrackContext> options)
            : base(options)
        {
        }
    }
}
