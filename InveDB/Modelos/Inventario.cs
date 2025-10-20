using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InveDB.Modelos
{
    [Table("Inventario")]
    public class Inventario
    {
        [Key]
        [Column("id_producto")]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        [Column("cantidad", TypeName = "decimal(10,2)")]
        public decimal Cantidad { get; set; }

        public Producto Producto { get; set; }
    }
}
