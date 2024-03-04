using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Models
{
    [Table("admin")]
    public partial class admin
    {
        public admin()
        {
            rentals = new HashSet<rental>();
        }

        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [StringLength(10)]
        public string Username { get; set; } = null!;

        [InverseProperty("CreatedByAdminNavigation")]
        public virtual ICollection<rental> rentals { get; set; }
    }
}
