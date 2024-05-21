﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PROG_POE_2.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Category")]
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }

        [Required]
        [DisplayName("Description")]
        [Column(TypeName = "nvarchar(150)")]
        public string description { get; set; }

        public int FarmerID { get; set; }
    }
}
