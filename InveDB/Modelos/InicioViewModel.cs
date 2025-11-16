using System;
using System.Collections.Generic;

namespace InveDB.Models
{
    public class InicioViewModel
    {
        public int CountUsuarios { get; set; }
        public int CountProductos { get; set; }
        public int CountMovimientos { get; set; }

        public List<UltimoMovimientoDto> Ultimos { get; set; } = new();

        public class UltimoMovimientoDto
        {
            public int Id { get; set; }
            public string Producto { get; set; }
            public DateTime Fecha { get; set; }
            public int Cantidad { get; set; }
            public string Tipo { get; set; } // "E" o "S"
        }
    }
}
