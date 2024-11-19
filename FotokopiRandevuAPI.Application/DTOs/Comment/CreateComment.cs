using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Comment
{
    public class CreateComment
    {
        public int StarRating { get; set; }
        public string CommentText { get; set; }
        public string OrderCode { get; set; }
    }
}
