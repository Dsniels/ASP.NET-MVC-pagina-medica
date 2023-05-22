using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        static string Conexion = "data source=localhost;initial catalog=DATOS;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework";
        private Models.CITA CITA = new Models.CITA();
        public ActionResult Index()
        {
            if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"])
            {
                ViewBag.IsLoggedIn = true;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Cita()
        {

            return View(CITA.Listar());
        }

        [HttpPost]
        public ActionResult Cita(string atencionmed, string nombre, string apellido, int? edad, String fecha, string telefono, string descripcion)
        {
            DateTime fechaCita;
            if (!DateTime.TryParse(fecha, out fechaCita))
            {
                ModelState.AddModelError("fecha", "El formato de fecha es incorrecto.");

            }
            if (ModelState.IsValid) // Validar los datos 
            {
                try // Ingresar los datos
                {
                    if (string.IsNullOrEmpty(atencionmed) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(telefono))
                    {
                        ViewBag.alerta = "danger";
                        ViewBag.res = "Todos los campos obligatorios deben ser llenados.";
                    }
                    else
                    {

                        if (CITA.Insertar(atencionmed, nombre, apellido, edad ?? 0, Convert.ToDateTime(fecha), telefono, descripcion))
                        {
                            ViewBag.alerta = "success";
                            ViewBag.res = "Cita registrada exitosamente.";
                        }
                        else
                        {
                            ViewBag.alerta = "danger";
                            ViewBag.res = "Error al registrar la cita :(";
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Datos de la cita no válidos.";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Mis_Citas()
        {

            return View(CITA.Listar());

        }

        public ActionResult Editar(int id)
        {
            return View(CITA.Registrar(id));
        }

        [HttpPost]
        public ActionResult Editar(int id, string atencionmed, string nombre, string apellido, int? edad, DateTime fecha, string telefono, string descripcion)
        {
            if (CITA.Actualizar(id, atencionmed, nombre, apellido, edad ?? 0 , Convert.ToDateTime(fecha), telefono, descripcion))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "Datos actualizados";
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Ocurrio un error :( ";
            }
            return View(CITA.Registrar(id));
        }

        public ActionResult Eliminar(int id)
        {
            if (CITA.Eliminar(id))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "Cita eliminada";
                return RedirectToAction("Mis_Citas", "Home");
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Ocurrio un error :(";
                return View(CITA.Registrar(id));
            }
        }


        public ActionResult Signup_page()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup_page(CLIENTE Usuario)
        {
            bool Registrado;
            string Mensaje;

            using (SqlConnection cn = new SqlConnection(Conexion))
            {

                SqlCommand cmd = new SqlCommand("registrar", cn);
                cmd.Parameters.AddWithValue("Nombre", Usuario.Nombre);
                cmd.Parameters.AddWithValue("Apellido", Usuario.Apellido);
                cmd.Parameters.AddWithValue("Correo", Usuario.Correo);
                cmd.Parameters.AddWithValue("Contraseña", Usuario.Contraseña);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;


                cn.Open();

                cmd.ExecuteNonQuery();

                Registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


            }
            ViewBag.alerta = "danger";
            ViewBag.res = "Datos de la cita no válidos.";
            ViewData["Mensaje"] = Mensaje;

            if (Registrado)
            {
                Session["LoggedIn"] = true;
                Session["Cliente"] = Usuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }


        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(CLIENTE Usuario)
        {


            using (SqlConnection cn = new SqlConnection(Conexion))
            {

                SqlCommand cmd = new SqlCommand("Validacion", cn);
                cmd.Parameters.AddWithValue("Correo", Usuario.Correo);
                cmd.Parameters.AddWithValue("Contraseña", Usuario.Contraseña);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                Usuario.IdCliente = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (Usuario.IdCliente != 0)
            {
                Session["LoggedIn"] = true;
                Session["Cliente"] = Usuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();
            }


        }

        public ActionResult Cerrar_sesion()
        {
            Session["Cliente"] = null;
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Articulo_1()
        {
            return View();
        }

        public ActionResult Articulo_2()
        {
            return View();
        }

        public ActionResult Articulo_3()
        {
            return View();
        }

        public ActionResult Articulo_4()
        {
            return View();
        }

    }
}