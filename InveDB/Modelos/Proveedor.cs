using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InveDB.Modelos
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [StringLength(20)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [StringLength(255)]
        [Column("direccion")]
        public string Direccion { get; set; }

        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
