using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PROG_POE_2.Models
{
    // generating a Farmer Table
    public class Farmer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Location { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Contact { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string EmployeeID { get; set; }

    }

}
