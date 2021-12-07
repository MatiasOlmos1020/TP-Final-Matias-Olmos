using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_Final_MatÍasOlmos.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public int precio { get; set; }
        public string descripcion { get; set; }
        public string urlImagen { get; set; }
        public bool favorito  { get; set; }
        public Categoria categoria { get; set; }
        public int categoriaId { get; set; }
        public Marca marca { get; set; }
        public int marcaId { get; set; }
        public int proveedorId { get; set; }
        public Proveedor proveedor { get; set; }
    }
}
