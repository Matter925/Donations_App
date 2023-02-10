using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Donations_App.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required , MaxLength(100)]
        public string Name { get; set; }

        [Required , MaxLength(2500)]
        public string Description { get; set; }

        public string ImageName { get; set; }
    }
}
