using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab9AzureStorage.Utils;

namespace Lab9AzureStorage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var almacen = ConfigurationManager.AppSettings["container"];
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var clave = ConfigurationManager.AppSettings["clave"];

            var st = new Storage(cuenta, clave);

            var l = st.ListaContenedor(almacen);

            return View(l);
        }

        public ActionResult Nueva()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nueva(HttpPostedFileBase fichero)
        {
            var almacen = ConfigurationManager.AppSettings["container"];
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var clave = ConfigurationManager.AppSettings["clave"];

            var st = new Storage(cuenta, clave);

            if (fichero != null && fichero.ContentLength > 0)
            {
                string nombre = DateTime.Now.Ticks.ToString();
                st.SubirFoto(fichero.InputStream, nombre, almacen);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Borrar(string id)
        {
            var almacen = ConfigurationManager.AppSettings["container"];
            var cuenta = ConfigurationManager.AppSettings["cuenta"];
            var clave = ConfigurationManager.AppSettings["clave"];

            var st = new Storage(cuenta, clave);
            st.BorrarFoto(id, almacen);

            return RedirectToAction("Index");
        }
    }
}