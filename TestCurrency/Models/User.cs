using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace TestCurrency.Models
{
    public class User: BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
    }
}
