using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Mono.Math;

namespace DAL.Models
{
    //[Bind(Exclude = "Id")]
    public class users
    {
        [Key]
        public Int64 Id { get; set; }
        public string  UserName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}