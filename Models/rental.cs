using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Models
{
    [Table("rental")]
    public partial class rental
    {
        [Key]
        public int id { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [StringLength(10)]
        public string MatricNo { get; set; } = null!;
        [StringLength(11)]
        [Unicode(false)]
        public string PhoneNo { get; set; } = null!;
        [Column(TypeName = "date")]
        [DisplayName("Rental Start Day")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RentalStartDay { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Rental End Day")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RentalEndDay { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        [DisplayName("Rental Fee")]
        public decimal RentalFee { get; set; }
        [DisplayName("Bicycle ID")]

        public int? BicycleId { get; set; }
        [DisplayName("Created By Admin")]

        public int? CreatedByAdmin { get; set; }
        [DisplayName("Created By Employee")]

        public int? CreatedByEmployee { get; set; }

        [ForeignKey("BicycleId")]
        [InverseProperty("rentals")]
        public virtual bicycle? Bicycle { get; set; }
        [ForeignKey("CreatedByAdmin")]
        [InverseProperty("rentals")]
        public virtual admin? CreatedByAdminNavigation { get; set; }
        [ForeignKey("CreatedByEmployee")]
        [InverseProperty("rentals")]
        public virtual employee? CreatedByEmployeeNavigation { get; set; }
    }
}
