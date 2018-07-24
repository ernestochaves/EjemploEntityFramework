using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Entities.Configuration
{
   
    public class DemoDBContext : DbContext
    {
        public DemoDBContext() : base("DemoDBContextConnection")
        {
            Database.SetInitializer<DemoDBContext>(null);
        }

         
        public DbSet<Item> Items { get; set; }
    }
}
