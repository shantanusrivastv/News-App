using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Model
{
    public class LinkDto
    {
        public string? Href { get; private set; }
        public string? Rel { get; private set; }
        public string Method { get; private set; } = "GET";

        public LinkDto(string? href, string? rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }

    public abstract class LinkedResourceBase
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

    public class HateoasResource<T>
    {
        public T? Data { get; set; }
        public ICollection<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
