using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Lab9AzureStorage.Utils
{
    public class Storage
    {
        private CloudStorageAccount cuentaAlmacenamiento;

        public Storage(string cuenta, string clave)
        {
            StorageCredentials cred = new StorageCredentials(cuenta, clave);
            cuentaAlmacenamiento = new CloudStorageAccount(cred, true);
        }

        private void ComprobarContainer(string contenedor)
        {
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);
            container.CreateIfNotExists();
        }

        public List<CloudBlockBlob> ListaContenedor(string contenedor)
        {
            ComprobarContainer(contenedor);
            CloudBlobClient cliente = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer carpeta = cliente.GetContainerReference(contenedor);

            List<CloudBlockBlob> urls = new List<CloudBlockBlob>();

            foreach (IListBlobItem item in carpeta.ListBlobs(null, false))
            {
                if (item is CloudBlockBlob)
                {
                    var blob = item as CloudBlockBlob;
                    urls.Add(blob);
                }
            }

            return urls;
        }

        public void SubirFoto(Stream foto, string nombre, string contenedor)
        {
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);

            CloudBlockBlob blobBlock = container.GetBlockBlobReference(nombre);

            blobBlock.UploadFromStream(foto);

            foto.Close();
        }

        public void BorrarFoto(string foto, string contenedor)
        {
            ComprobarContainer(contenedor);
            CloudBlobClient cliente = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer carpeta = cliente.GetContainerReference(contenedor);
            CloudBlockBlob blockBlob = carpeta.GetBlockBlobReference(foto);

            blockBlob.Delete();
        }

    }
}