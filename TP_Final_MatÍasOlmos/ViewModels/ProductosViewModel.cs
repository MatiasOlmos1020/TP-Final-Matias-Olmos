using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP_Final_MatÍasOlmos.Models;

namespace TP_Final_MatÍasOlmos.ViewModels
{
    public class ProductosViewModel
    {
        public List<Producto> productos { get; set; }
        public SelectList categorias { get; set; }
        public string nombre { get; set; }
        public SelectList marca { get; set; }

        //public ICollection<Proveedor> proveedores { get; set; }

    }
}
