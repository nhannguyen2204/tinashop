using System.Collections.Generic;

namespace TinaShopV2.Models
{
    public class MessageSummary
    {
        public MessageSummary()
        {
            Successes = new List<string>();
            Errors = new List<string>();
        }

        public List<string> Successes { get; set; }
        public List<string> Errors { get; set; }
    }
}