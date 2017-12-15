using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bgrimmett_portfolio2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateReason { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}