using System.ComponentModel.DataAnnotations;

namespace N5PermisosAPI.Models
{
    public class TipoPermiso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }
    }
}
