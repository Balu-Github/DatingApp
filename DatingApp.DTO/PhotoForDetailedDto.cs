using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.DTO
{
    public class PhotoForDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }        
        public int UserId { get; set; }
        public string PublicId { get; set; }
    }
}
