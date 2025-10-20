namespace InveDB.Modelos
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public int? IdCategoria { get; set; }
        public string? Proveedor { get; set; }
        public int? IdProveedor { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? UnidadMedida { get; set; }
        public decimal Existencia { get; set; }
    }
}
