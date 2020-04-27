using FreeStuff4.Controllers;
using FreeStuff4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeStuff4.Models
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public bool CanDelete { get; set; }
    }
    
}
