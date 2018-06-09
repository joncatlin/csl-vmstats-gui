using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VmstatsGUI
{
    public class Request
    {
        public int ID { get; set; }

        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }      // The start of the date range for the stats to process

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }        // The end of the date range for the stats to process

        [Display(Name = "Process Pipeline")]
        public string Dsl { get; set; }             // The Domain Specific Language for processing the selected stats

        [Display(Name = "Pattern to select virtual machines")]
        public string VmPattern { get; set; }      // The VM pattern used to select the stats for
    }
}
