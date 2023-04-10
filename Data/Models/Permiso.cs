using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace N5PermisosAPI.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreEmpleado { get; set; }

        [Required]
        public string ApellidoEmpleado { get; set; }

        [Required]
        public int TipoPermisoId { get; set; }

        [Required]
        public DateTime FechaPermiso { get; set; }

        
        public TipoPermiso TipoPermiso { get; set; }
    }
}
