using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class PJVertDto
    {
        public int Id { get; set; }
        public int RecID { get; set; }
        public int IdEmpresa { get; set; }
        public string Numero { get; set; }
        public string Empresa { get; set; }
        public string RFC { get; set; }
        public double zAGE { get; set; }
        public double zERAGE { get; set; }
        public double zASV { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public double Monto { get; set; }
    }
}
