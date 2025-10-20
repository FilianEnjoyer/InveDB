using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InveDB.Modelos
{
    [Table("Sucursal")]
    public class Sucursal
    {
        [Key]
        [Column("id_sucursal")]
        public int IdSucursal { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [StringLength(255)]
        [Column("direccion")]
        public string Direccion { get; set; }

        [StringLength(20)]
        [Column("telefono")]
        public string Telefono { get; set; }

        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
