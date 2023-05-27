namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;

    [Table("CITA")]
    public partial class CITA
    {
        [Key]
        public int IdCita { get; set; }

        [StringLength(30)]
        public string AtencionMedica { get; set; }
       
        public DateTime Fecha { get; set; }

        [StringLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }

        public int Edad { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        public int? IdCliente { get; set; }

        [StringLength(100)]
        public string Telefono { get; set; }

        public virtual CLIENTE CLIENTE { get; set; }

        public List<CITA> Listar(int IdCliente)
        {
            var citas = new List<CITA>();
            string connectionString = "data source=Localhost;initial catalog=DATOS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            string query = "SELECT * FROM CITA WHERE IdCliente = @IdCliente;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@IdCliente", System.Data.SqlDbType.Int); // @Parámetro, Tipo
                        command.Parameters["@IdCliente"].Value = IdCliente;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cita = new CITA
                                {
                                    Fecha = (DateTime)reader["Fecha"],
                                    Nombre = (string)reader["Nombre"],
                                    Apellido = (string)reader["Apellido"],
                                    AtencionMedica = (string)reader["AtencionMedica"],
                                    Edad = (int)reader["Edad"],
                                    IdCita = (int)reader["IdCita"]
                                };

                                citas.Add(cita);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al obtener las citas: " + ex.Message);
            }

            return citas;
        }

        public bool Insertar(int IdCliente, string atencionmed, string nombre, string apellido, int edad, DateTime fecha, string telefono, string descripcion)
        {
            bool estado = false;

            try
            {
                using (var cnx = new Model1())
                {
                    string query = "INSERT INTO CITA (IdCliente, AtencionMedica, Nombre, Apellido, Edad, Fecha, Telefono, Descripcion) " +
                                   "VALUES (@IdCliente, @AtencionMedica, @Nombre, @Apellido, @Edad, @Fecha, @Telefono, @Descripcion)";

                    int r = cnx.Database.ExecuteSqlCommand(query,
                        new SqlParameter("@IdCliente", IdCliente),
                        new SqlParameter("@AtencionMedica", atencionmed),
                        new SqlParameter("@Nombre", nombre),
                        new SqlParameter("@Apellido", apellido),
                        new SqlParameter("@Edad", edad),
                        new SqlParameter("@Fecha", fecha),
                        new SqlParameter("@Telefono", telefono),
                        new SqlParameter("@Descripcion", descripcion));

                    if (r == 1)
                    {
                        estado = true;
                    }
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }

            return estado;
        }
        public CITA filtrar(int id)
        {
            var registro = new CITA();
            try
            {
                using (var cnx = new Model1())
                {
                    registro = cnx.CITA.SingleOrDefault(a => a.IdCita == id);
                }
            }
            catch (Exception)
            {

            }
            return registro;
        }

        public bool Actualizar(CITA datos, int id)
        {
            bool estado = false;
            string query = "UPDATE CITA SET AtencionMedica = @AtencionMedica, Nombre = @Nombre, Apellido = @Apellido, Edad = @Edad, Fecha = @Fecha, Telefono = @Telefono, Descripcion = @Descripcion WHERE IdCita ="+ id;
            string Descripcion = datos.Descripcion == null ? string.Empty:datos.Descripcion;
            try
            {
                using (var cnx = new Model1())
                {
                    int CAMBIOS = cnx.Database.ExecuteSqlCommand(query,
                        new SqlParameter("@AtencionMedica", datos.AtencionMedica),
                        new SqlParameter("@Nombre",         datos.Nombre),
                        new SqlParameter("@Apellido",       datos.Apellido),
                        new SqlParameter("@Edad",           datos.Edad),
                        new SqlParameter("@Fecha",          datos.Fecha),
                        new SqlParameter("@Telefono",       datos.Telefono),
                        new SqlParameter("@Descripcion",    Descripcion));
                    

                    if (CAMBIOS >= 1)
                    {
                        estado = true;
                    }
                }
            }
            catch (Exception)
            {

            }

            return estado;
        }



        public Boolean Eliminar(int id)
        {
            bool estado = false;
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("DELETE FROM CITA WHERE IdCita=" + id);
                    if (r == 1)
                    {
                        estado = true;
                    }
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }
            return estado;
        }


    }
}
