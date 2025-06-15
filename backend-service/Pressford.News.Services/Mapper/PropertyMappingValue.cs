using System;
using System.Collections.Generic;

namespace Pressford.News.Services.Mapper
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; }
        public bool ReverseDirection { get; }

        public PropertyMappingValue(IEnumerable<string> destinations, bool reverseDirection = false)
        {
            DestinationProperties = destinations ?? throw new ArgumentNullException(nameof(destinations));
            ReverseDirection = reverseDirection;
        }
    }

}
