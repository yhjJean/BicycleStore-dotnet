using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Models
{
    [Table("employee")]
    public partial class employee
    {
        public employee()
        {
            rentals = new HashSet<rental>();
        }

        [Key]
        public int id { get; set; }
        [StringLength(10)]
        public string Username { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [StringLength(30)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [InverseProperty("CreatedByEmployeeNavigation")]
        public virtual ICollection<rental> rentals { get; set; }
    }
}
