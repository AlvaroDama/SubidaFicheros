using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubidaFicheros.Controllers;
using SubidaFicheros.Models;

namespace TestMVC
{
    public abstract class BasePruebas
    {
        protected HomeController Controller;
        protected Ficheros_Entities db;

        protected BasePruebas()
        {
            db = new Ficheros_Entities();
            Controller = new HomeController();
        }


    }
}
