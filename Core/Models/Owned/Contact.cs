using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VEFA.Core.Models.Owned
{
    [Owned]
    public class Contact
    {
        [Required]
        [StringLength(255)]
        public string ContactName { get; set; }
        
        [StringLength(255)]
        public string ContactPhone { get; set; }

        [Required]
        [StringLength(255)]
        public string ContactEmail { get; set; }
    }
}
