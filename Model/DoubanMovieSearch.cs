using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanMovieSearch : DoubanSearchBase
    {
        public List<DoubanMovie> Movies { get; set; }
        public List<DoubanMovie> Subjects { get; set; }
    }
}
