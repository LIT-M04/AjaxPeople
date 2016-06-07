using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ajax.PeopleData;

namespace MvcApplication28.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Person> People { get; set; } 
    }
}