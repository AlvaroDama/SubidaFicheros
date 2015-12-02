using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SubidaFicheros.Utiles
{
    public class GestionarFicheros
    {
        public static string GuardarFicheroDisco(HttpPostedFileBase fichero, 
            HttpServerUtilityBase server)
        {
            var id = Guid.NewGuid(); //--> identificador único
            // var n = DateTime.Now.Ticks; --> saca la firma horaria.
            string nombre = null;

            if (fichero != null && fichero.ContentLength > 0)
            {
                var ext = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".") + 1);
                nombre = $"{id}.{ext}"; //--> string.Format("{0}.{1}", id, ext);

                fichero.SaveAs(server.MapPath("/Ficheros") + "/" + nombre);
            }

            return nombre;
        }

        public static byte[] ToBinario(HttpPostedFileBase fichero)
        {
            byte[] data = null;

            if (fichero != null && fichero.ContentLength > 0)
            {
                using (var stream = fichero.InputStream) //Input|Output Stream es genérico.
                {
                    var ms = stream as MemoryStream; //nos llevamos el Stream a memoria.

                    if (ms == null)
                    {
                        ms = new MemoryStream(); //si no existe, lo forzamos
                        stream.CopyTo(ms);
                    }

                    data = ms.ToArray();
                }
                
            }

            return data;
        }
    }
}
