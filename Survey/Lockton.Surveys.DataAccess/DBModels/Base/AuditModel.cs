using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels.Base
{
    public class AuditModel
    {
        [Column("Seguridad_UsuarioCreacion")]
        public int? CreateUserID { get; set; }
        [Column("Seguridad_FechaCreacion")]
        public DateTime? CreateDate { get; set; }
        [Column("Seguridad_UsuarioModificacion")]
        public int? UpdateUserID { get; set; }
        [Column("Seguridad_FechaModificacion")]
        public DateTime? UpdateDate { get; set; }
    }
}
