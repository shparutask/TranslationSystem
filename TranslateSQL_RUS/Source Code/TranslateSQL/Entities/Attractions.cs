using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateSQL.Entities
{
    public class Attraction : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class Monument : Attraction { }

    public class Homestead : Attraction { }

    public class Museum : Attraction
    {
        public string OpeningHours { get; set; }

        public string ClosingHours { get; set; }

        public string WorkingDays { get; set; }
    }

    public class Park : Attraction
    {
        public string OpeningHours { get; set; }

        public string ClosingHours { get; set; }

        public string WorkingDays { get; set; }
    }
}
