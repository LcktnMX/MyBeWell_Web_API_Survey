using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels
{
    [Table("Seguridad_EstatusUsuario")]
    public class UserStatus
    {
        [Key]
        [Column("Seguridad_EstatusUsuario_Id")]
        public int Id { get; set; }

        [Column("Seguridad_EstatusUsuario_Tipo")]
        public string  Nombre{ get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}
