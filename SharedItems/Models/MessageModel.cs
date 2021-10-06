using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class MessageModel
    {
        //public MessageModel()
        //{
        //    GroupName = "mainChat";
        //}

        public int Id { get; set; }
        public int UserModelId { get; set; }
        public int ChatGroupModelId { get; set; }

        public DateTime Time { get; set; }
        public bool IsFirstMessage { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public virtual UserModel UserModel { get; set; }

        [JsonIgnore]
        public virtual ChatGroupModel ChatGroupModel { get; set; }
    }
}
