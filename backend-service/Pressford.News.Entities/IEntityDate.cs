using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Entities
{
    public interface IEntityDate
    {
        public DateTime DatePublished { get; set; }

        public DateTime DateModified { get; set; }
    }
}