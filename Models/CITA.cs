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

    [Table("CITA")]
    public partial class CITA
    {
        [Key]
        public int IdCita { get; set; }

        [StringLength(30)]
        public string AtencionMedica { get; set; }

        public DateTime Fecha { get; set; }

        [StringLength(300)]
        public string Descripcion { get; set; }

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

        public List<CITA> Listar()
        {
            var citas = new List<CITA>();
            string query = "SELECT * FROM CITA";
            string connectionString = "data source=Localhost;initial catalog=DATOS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
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

        public bool Insertar(string atencionmed, string nombre, string apellido, int edad, DateTime fecha, string telefono, string descripcion)
        {
            bool estado = false;

            try
            {
                using (var cnx = new Model1())
                {
                    string query = "INSERT INTO CITA (AtencionMedica, Nombre, Apellido, Edad, Fecha, Telefono, Descripcion) " +
                                   "VALUES (@AtencionMedica, @Nombre, @Apellido, @Edad, @Fecha, @Telefono, @Descripcion)";

                    int r = cnx.Database.ExecuteSqlCommand(query,
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
        public CITA Registrar(int id)
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

        public bool Actualizar(int id, string atencionmed, string nombre, string apellido, int? edad, DateTime fecha, string telefono, string descripcion)
        {
            bool estado = false;
            string consulta = "UPDATE CITA SET AtencionMedica = @AtencionMedica, Nombre = @Nombre, Apellido = @Apellido, Edad = @Edad, Fecha = @Fecha, Telefono = @Telefono, Descripcion = @Descripcion WHERE IdCita = @IdCita";

            try
            {
                using (var cnx = new Model1())
                {
                    int filasAfectadas = cnx.Database.ExecuteSqlCommand(consulta,
                        new SqlParameter("@IdCita", id),
                        new SqlParameter("@AtencionMedica", atencionmed),
                        new SqlParameter("@Nombre", nombre),
                        new SqlParameter("@Apellido", apellido),
                        new SqlParameter("@Edad", edad ?? (object)DBNull.Value), // Manejar el valor nulo
                        new SqlParameter("@Fecha", fecha),
                        new SqlParameter("@Telefono", telefono),
                        new SqlParameter("@Descripcion", descripcion));

                    if (filasAfectadas >= 1)
                    {
                        estado = true;
                    }
                }
            }
            catch (SqlException )
            {
                // Manejar excepciones específicas de SQL Server
                // Loggear o notificar el error según corresponda
                // ...
            }
            catch (Exception)
            {
                // Manejar excepciones genéricas u otras excepciones específicas
                // Loggear o notificar el error según corresponda
                // ...
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
