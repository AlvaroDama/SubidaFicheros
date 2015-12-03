using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SubidaFicheros.Utiles
{
    public class AzureStorageUtils
    {
        private CloudStorageAccount _cuenta;
        private string _contenedor;

        public AzureStorageUtils(string cuenta, string clave, string contenedor)
        {
            StorageCredentials sc= new StorageCredentials(cuenta, clave);
            _cuenta = new CloudStorageAccount(sc, true);
            _contenedor = contenedor;
        }

        private void ComprobarContenedor(string contenedor = null)
        {
            if (contenedor != null)
            {
                _contenedor = contenedor;
            }

            var cliente = _cuenta.CreateCloudBlobClient(); //crea un cliente 
            var cont = cliente.GetContainerReference(contenedor); //if (contenedor == null) --> cont = null
            cont.CreateIfNotExists();
        }

        public void SubirFichero(Stream fichero, string nombre, string contenedor = null)
        {
            ComprobarContenedor(contenedor);

            var cliente = _cuenta.CreateCloudBlobClient();
            var cont = cliente.GetContainerReference(_contenedor);
            var blob = cont.GetBlockBlobReference(nombre);
            blob.UploadFromStream(fichero); //podría hacerse upload de array binario, texto, etc
            fichero.Close();
        }

        public byte[] RecuperarArchivo(string nombre, string contenedor)
        {
            ComprobarContenedor(contenedor);

            var cliente = _cuenta.CreateCloudBlobClient();
            var cont = cliente.GetContainerReference(_contenedor);
            var blob = cont.GetBlockBlobReference(nombre);

            blob.FetchAttributes(); //obtiene las propiedades del Blob
            var len = blob.Properties.Length;
            var dest = new byte[len];
            
            blob.DownloadToByteArray(dest, 0);

            return dest;
        }

    }
}
