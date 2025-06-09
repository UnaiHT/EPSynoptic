using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public string Filename { get; set; }

        public string Title { get; set; }

        public string OwnerId { get; set; }

        public DateTime Date { get; set; }
    }
}
