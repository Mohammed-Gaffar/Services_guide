using Core.Entities.Base;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [DisplayName("اسم الستخدم")]
        public string UserName { get; set; }
        [DisplayName("صلاحية المستخدم")]
        [Required]
        public Roles Role { get; set; }
    }
}
