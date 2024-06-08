using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BVHAU.VMM
{
    public class UserConfig
    {
        public string VmrunPath { get; set; }
        public int IntervalTimeMs { get; set; }
        public List<VM> Vms { get; set; }
        
    }
}
