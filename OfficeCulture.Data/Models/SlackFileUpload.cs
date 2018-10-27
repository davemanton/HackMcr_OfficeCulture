using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCulture.Data.Models
{
    public class SlackFileUpload
    {
        public string token { get; set; }
        public string channels { get; set; }
        public File file { get; set; }
        public string filetype { get; set; }
        public string initial_comment { get; set; }
        public string title { get; set; }
    }
}
