using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public DateTime? Create_At { get; set; }
        public int Created_by { get; set; }
        public DateTime? Update_At { get; set; }
        public int? Updated_by { get; set; }
    }
}
