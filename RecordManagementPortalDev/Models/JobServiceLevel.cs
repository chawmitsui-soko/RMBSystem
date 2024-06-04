using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class JobServiceLevel
    {
        [Key]
        public int Id { get; set; }        
        public int SvrId { get; set; }  
        public string SrvLevel { get; set; }        

    }
}
