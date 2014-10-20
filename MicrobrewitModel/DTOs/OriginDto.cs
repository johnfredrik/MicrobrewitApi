using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class OriginDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get { return "origin"; } }
    }
}
