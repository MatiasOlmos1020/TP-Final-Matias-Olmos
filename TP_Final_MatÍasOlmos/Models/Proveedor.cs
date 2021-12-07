using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_Final_MatÍasOlmos.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public int telefono { get; set; }
        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string Provincia { get; set; }
    }
}
