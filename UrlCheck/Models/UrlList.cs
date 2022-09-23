using System;
using System.Collections.Generic;

namespace UrlCheck.Models
{
    public partial class UrlList
    {
        public UrlList()
        {
            UrlCheckLogs = new HashSet<UrlCheckLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUserEmail { get; set; } = null!;

        public virtual ICollection<UrlCheckLog> UrlCheckLogs { get; set; }
    }
}
