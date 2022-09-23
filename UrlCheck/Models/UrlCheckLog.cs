using System;
using System.Collections.Generic;

namespace UrlCheck.Models
{
    public partial class UrlCheckLog
    {
        public int Id { get; set; }
        public int UrlListId { get; set; }
        public string Url { get; set; } = null!;
        public bool IsUrlUp { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUserEmail { get; set; } = null!;

        public virtual UrlList UrlList { get; set; } = null!;
    }
}
