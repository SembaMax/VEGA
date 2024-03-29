﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VEFA.Core.Models
{
    [Table("Models")]
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public String Name { get; set; }
        public Make Make { get; set; }
        public int MakeId { get; set; }
    }
}
