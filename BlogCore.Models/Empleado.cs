using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Display(Name = "Salario")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un valor positivo")]
        public decimal Salario { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Número de Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string NumeroTelefono { get; set; }

        [Display(Name = "Fecha de Contratación")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }

        [Display(Name = "Departamento")]
        public string Departamento { get; set; }

        [Display(Name = "Cargo")]
        public string Cargo { get; set; }
    }
}