using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateSQL.Entities
{
    public class Address : Entity
    {
        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

        public string Area { get; set; }
    }
}
