using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InveDB.Modelos
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [StringLength(255)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        [Column("unidad_medida")]
        public string UnidadMedida { get; set; }

        [Required]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }

        public ICollection<Inventario> Inventarios { get; set; }
        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
