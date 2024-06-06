using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Service :BaseEntity
    {
        [Required]
        [DisplayName("اسم الخدمة")]
        public string Name { get; set; }
        [Required]
        [DisplayName("وصف الخدمة")]
        public string Description { get; set; }
        [Required]
        [DisplayName("رابط الخدمة")]
        public string Link { get; set; }
        [DisplayName("حالة الخدمة")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
