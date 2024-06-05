using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        [DisplayName("اسم المستخدم")]
        public string name { get; set; }
        [DisplayName("اسم الستخدم")]
        public string UserName { get; set; }
        [DisplayName("البريد الإلكتروني")]
        public string email { get; set; }
        [DisplayName("كلمة المرور")]
        public string password { get; set; }
        [DisplayName("صلاحية المستخدم")]
        public string Role { get; set; }
    }
}
