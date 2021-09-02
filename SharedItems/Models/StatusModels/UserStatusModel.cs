using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models.StatusModels
{
    public class UserStatusModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public BanStatusModel BanStatus { get; set; }
        public MuteStatusModel MuteStatus { get; set; }
    }
}
