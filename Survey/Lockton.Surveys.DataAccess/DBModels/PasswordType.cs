using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels
{
    [Table("Seguridad_TipoPassword")]
    public class PasswordType
    {
        [Key]
        [Column("Seguridad_TipoPassword_Id")]
        public int Id { get; set; }
        [Column("Seguridad_TipoPassword_Tipo")]
        public string  Nombre{ get; set; }
        [Column("Seguridad_TipoPassword_Expresion")]
        public string Expression { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
