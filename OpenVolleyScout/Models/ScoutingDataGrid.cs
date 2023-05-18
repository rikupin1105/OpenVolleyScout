using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVolleyScout.Models
{
    public class ScoutingDataGrid
    {
        public string RallyId { get; set; }
        public int Set { get; set; }
        public string? ParsedCommand { get; set; }

        public string Zone { get; set; }
    }
}
