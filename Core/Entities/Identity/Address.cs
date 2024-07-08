using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Entities.Identity
{
    public class Address
    {
       public int Id { get; set; } 
       public string FirstName { get; set; }
       public string  LastName { get; set; }
       public string  Street { get; set; }
       public string City { get; set; }
       public string  State { get; set; }
       public int ZipCode { get; set; }
       [Required]
       public string AppUserId { get; set; }
       public AppUser AppUser { get; set; } 

    }
}