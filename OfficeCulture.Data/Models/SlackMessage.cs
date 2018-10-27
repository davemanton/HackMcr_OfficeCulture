using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCulture.Data.Models
{
    public class SlackMessage
    {
        public string text { get; set; }
        public File file { get; set; }
        public List<Attachment> attachments { get; set; }
    }
}
