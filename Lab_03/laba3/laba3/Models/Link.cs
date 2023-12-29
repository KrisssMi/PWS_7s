namespace laba3.Controllers
{
    public class Link  
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Action { get; set; }

        public Link(string rel, string href, string action)
        {
            Rel = rel;
            Href = href;
            Action = action;
        }

        public Link()
        {

        }
    }
}