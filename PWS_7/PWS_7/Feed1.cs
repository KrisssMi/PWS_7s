using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace PWS_7
// http://localhost:40124/PWS_7/Feed1/students/2/notes/json?format=rss
{
    public class Feed1 : IFeed1
    {
        public SyndicationFeedFormatter CreateFeed()
        {
            // Создать новый веб-канал.
            var feed = new SyndicationFeed("Feed Title", "A WCF Syndication Feed", null);
            var items = new List<SyndicationItem>();

            // Создать новый элемент рассылки.
            var item = new SyndicationItem("An item", "Item content", null);
            items.Add(item);
            feed.Items = items;

            // Возвращать канал ATOM или RSS, основываясь на строке запроса
            var query = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["format"];
            SyndicationFeedFormatter formatter = null;
            if (query == "atom")
            {
                formatter = new Atom10FeedFormatter(feed);
            }
            else
            {
                formatter = new Rss20FeedFormatter(feed);
            }

            return formatter;
        }
        
        public SyndicationFeedFormatter GetStudentNotes(string studentId, string formatRequest)
        {
            var feed = new SyndicationFeed("Subjects & Notes", "Get list of notes by all subjects for this student", null);
            var items = new List<SyndicationItem>();

            var url =
                $"http://localhost:49240/WcfDataService1.svc/Note?$filter=student_id eq {studentId}&$select=subject,note1";
            if (formatRequest == "json")
            {
                url += "&$format=json";
            }
    
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var responseString = reader.ReadToEnd();

                if (formatRequest == "json")
                {
                    var notesResp = JsonConvert.DeserializeObject<NoteResponse>(responseString);
                    var notes = notesResp.Value;
                    foreach (var note in notes)
                    {
                        items.Add(new SyndicationItem(note.Subj, note.Note1.ToString(), null));
                    }
                }
                else if (formatRequest == "atom")
                {
                    var xmlReader = XmlReader.Create(new StringReader(responseString));
                    var atomFeed = SyndicationFeed.Load(xmlReader);
                    
                    items.AddRange(atomFeed.Items);
                }
            }

            feed.Items = items;

            var query = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["format"];
            SyndicationFeedFormatter formatter = null;
            if (query == "atom")
            {
                formatter = new Atom10FeedFormatter(feed);
            }
            else
            {
                formatter = new Rss20FeedFormatter(feed);
            }
            return formatter;
        }
    }
}
