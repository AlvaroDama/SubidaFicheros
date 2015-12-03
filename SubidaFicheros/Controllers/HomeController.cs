using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using SubidaFicheros.Models;
using SubidaFicheros.Utiles;

namespace SubidaFicheros.Controllers
{
    public class HomeController : Controller
    {

        List<TipoFichero> tipos = new List<TipoFichero>()
        {
            new TipoFichero() {Id = 1, Nombre = "Imagen"},
            new TipoFichero() {Id = 2, Nombre = "Otra cosa"}
        };
        Ficheros_Entities db = new Ficheros_Entities();

        // GET: Home
        public ActionResult Index()
        {
            var data = db.Ficheros;
            return View(data);
        }

        //almacenaje --> determina cómo (dónde) guardaremos:
        //1) Local
        //2) 
        //3) Binary 
        //4) Nube
        public ActionResult Subida(int almacenaje = 1)
        {
            var tipoAlmacen = "";
            if (almacenaje == 1)
                tipoAlmacen = "interno";
            if (almacenaje == 2)
                tipoAlmacen = "base64";
            if (almacenaje == 3)
                tipoAlmacen = "binario";
            if (almacenaje == 4)
                tipoAlmacen = "azure";


            ViewBag.almacenaje = tipoAlmacen;
            ViewBag.TipoFichero = new SelectList
                (tipos, "Id", "Nombre"); // params(lista, valor que guarda, texto del dropbox)
            return View(new Ficheros());
        }

        public FileResult DownloadFile(int id, int tipo = 0)
        {
            byte[] fichero;
            var f = db.Ficheros.Find(id);
            if (tipo == 0)
            {
                fichero = Convert.FromBase64String(f.Datos);
            }
            else
            {
                fichero = f.DatosB;
            }

            return File(fichero, MediaTypeNames.Application.Octet, f.Nombre);
        }

        [HttpPost]
        public ActionResult Subida(Ficheros model, HttpPostedFileBase fichero)
            // Cada control File del html envía un objeto de tipo HttpPostedFileBase. 
            // HttpPostedFileBase es un string con t0do el flujo de datos del fichero.
        {
            if (model.Tipo == "interno")
            {
                var n = GestionarFicheros.GuardarFicheroDisco(fichero, Server);

                if (n != null)
                {
                    model.Datos = n;
                    model.DatosB = new byte[1];
                    db.Ficheros.Add(model);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else if (model.Tipo == "base64")
            {
                var data = GestionarFicheros.ToBinario(fichero);
                if (data != null)
                {
                    model.Datos = Convert.ToBase64String(data);
                    model.DatosB = new byte[1];
                    model.Nombre = fichero.FileName;
                    db.Ficheros.Add(model);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else if (model.Tipo == "binario")
            {
                var datab = GestionarFicheros.ToBinario(fichero);
                if (datab != null)
                {
                    model.Datos = "";
                    model.DatosB = datab;
                    model.Nombre = fichero.FileName;
                    db.Ficheros.Add(model);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else if (model.Tipo == "azure")
            {
                var cuenta = ConfigurationManager.AppSettings["CuentaAS"];
                var cliente = ConfigurationManager.AppSettings["ClaveAS"];
                var contenedor = ConfigurationManager.AppSettings["ContenedorAS"];
                
                var az = new AzureStorageUtils(cuenta, cliente, contenedor);

                var n = Guid.NewGuid();
                var ext = fichero.FileName.Substring(fichero.FileName.LastIndexOf("."));

                az.SubirFichero(fichero.InputStream, n + ext);
            }

            return RedirectToAction("Index");
        }
    }
}