using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecordManagementPortalDev.Models
{
    public class JobsDetLoc
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int Id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JDId { get; set; }
        [DisplayName("Job Registration No")]
        public string? JobNo { get; set; }
        [DisplayName("Customer Code")]
        [Required]
        public string CustCode { get; set; }
        [DisplayName("Department Code")]
        [Required]
        public string DeptCode { get; set; }
        [DisplayName("Cartons")]
        [Required]
        public string Cartons { get; set; }
        [DisplayName("Job Level")]
        [Required]
        public string JobLevel { get; set; }
        [DisplayName("File No")]
        [Required]
        public string FileNo { get; set; }
        [DisplayName("Scanner Date")]
        [Required]
        public DateTime ScannerDate { get; set; }
        [DisplayName("Control")]
        public string? Control { get; set; }
        [DisplayName("Location")]
        [Required]
        public string Location { get; set; }
        [DisplayName("Status")]
        public string? Status { get; set; }
        [DisplayName("Seal No")]
        public string? SealNo { get; set; }
        [DisplayName("Product Code")]
        public string? ProductCode { get; set; }
        [DisplayName("Staff")]
        [Required]
        public string Staff { get; set; }
        [DisplayName("Description")]       
        public string? Description { get; set; }
        [DisplayName("Creted Date")]
        [Required]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Updated Date")]
        [Required]
        public DateTime UpdatedDate { get; set; }

    }
}

