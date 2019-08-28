using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    [Table("aspnetusers")]
    public class AspNetUsers
    {
        public string id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        public int user_type_id { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public bool? email_confirmed { get; set; }
        public string password_hash { get; set; }
        public string security_stamp { get; set; }
        public string phone_number { get; set; }
        public bool phone_number_confirmed { get; set; }
        public bool two_factor_enabled { get; set; }
        public DateTime? lock_out_enddate_utc { get; set; }
        public bool lock_out_enabled { get; set; }
        public int access_failed_count { get; set; }
        public string username { get; set; }
        public bool? is_hiring_manager { get; set; }

        [NotMapped]
        public string password { get; set; }
    }
}
