using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class LogScanPack
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("File Name")]
        [Required]
        public string FileName { get; set; }
        [DisplayName("Staff ID")]
        [Required]
        public string StaffId { get; set; }
        [DisplayName("Scan Date")]
        [Required]
        public DateTime ScanDate { get; set; }
        [DisplayName("Total Scan Data")]
        [Required]
        public int TotalSData { get; set; }
        [DisplayName("File Location")]
        [Required]
        public string Location { get; set; }
        [DisplayName("Remark")]
        [Required]
        public string Remark { get; set; }
    }
}
