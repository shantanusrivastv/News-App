using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services.Mapper
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; }
        public bool RevertDirection { get; }

        public PropertyMappingValue(IEnumerable<string> destinations, bool revert = false)
        {
            DestinationProperties = destinations ?? throw new ArgumentNullException(nameof(destinations));
            RevertDirection = revert;
        }
    }

}
