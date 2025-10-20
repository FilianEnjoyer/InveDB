namespace InveDB.Modelos
{
    public class InventarioViewModel
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public decimal Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Descripcion { get; set; }
        public string Proveedor { get; set; }
        public DateTime? UltimaFecha { get; set; }
    }
}
