using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels
{
    [Table("Seguridad_HistorialPassword")]
    public class PasswordHistory
    {
        [Key]
        [Column("Seguridad_HistorialPassword_Id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("Seguridad_HistorialPassword_UsuarioId")]
        public int UserId { get; set; }

        [Column("Seguridad_HistorialPassword_Password")]
        public string Password { get; set; }

        [Column("Seguridad_HistorialPassword_Fecha")]
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}
