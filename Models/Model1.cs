using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WEB.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<CITA> CITA { get; set; }
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CITA>()
                .Property(e => e.AtencionMedica)
                .IsUnicode(false);

            modelBuilder.Entity<CITA>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<CITA>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<CITA>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<CITA>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTE>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTE>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTE>()
                .Property(e => e.Correo)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTE>()
                .Property(e => e.Contraseña)
                .IsUnicode(false);
        }
    }
}
