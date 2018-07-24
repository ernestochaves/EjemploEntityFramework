using Demo.Entities;
using Demo.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Try and create some items");

            using (DemoDBContext db = new DemoDBContext())
            {
                try
                {
                    //Se crean un elemento cualquiera
                    Item item1 = new Item()
                    {
                        Name = "Item1" + DateTime.Now.ToShortDateString(),
                        Description = "Description:" + DateTime.Now.ToLongDateString()
                    };

                    //Se agregan a la coleccion en el db context
                    db.Items.Add(item1);

                    //Se guardan los cambios
                    db.SaveChanges();
                    System.Console.WriteLine("Item CREATED!!!");
                    System.Console.ReadKey();

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Something bad happened :(");
                    System.Console.Write(ex);
                    System.Console.ReadKey();
                }
            }



            //Then we can do the same to list the items in the database
            System.Console.WriteLine("/n Items in the database so far:");
            using (DemoDBContext db = new DemoDBContext())
            {
                foreach (var item in db.Items)
                {
                    System.Console.WriteLine("---Item---");
                    System.Console.WriteLine("Id: " + item.Id);
                    System.Console.WriteLine("Name: " + item.Name);
                    System.Console.WriteLine("Description: " + item.Description);

                }

                System.Console.ReadKey();
            }


        }
    }
}
