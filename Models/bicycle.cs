using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Models
{
    [Table("bicycle")]
    public partial class bicycle
    {
        public bicycle()
        {
            rentals = new HashSet<rental>();
        }

        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Type { get; set; } = null!;
        public int Status { get; set; }

        [InverseProperty("Bicycle")]
        public virtual ICollection<rental> rentals { get; set; }
    }
}
