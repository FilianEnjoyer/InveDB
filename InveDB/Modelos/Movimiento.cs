using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InveDB.Modelos
{
    [Table("Movimiento")]
    public class Movimiento
    {
        [Key]
        [Column("id_movimiento")]
        public int IdMovimiento { get; set; }

        [Required]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(1)]
        [Column("tipo")]
        [RegularExpression("[ES]", ErrorMessage = "El tipo debe ser 'E' (Entrada) o 'S' (Salida)")]
        public string Tipo { get; set; }

        [Required]
        [Column("cantidad", TypeName = "decimal(10,2)")]
        public decimal Cantidad { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        [Column("id_sucursal")]
        public int? IdSucursal { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }

        [ForeignKey("IdSucursal")]
        public Sucursal Sucursal { get; set; }
    }
}
