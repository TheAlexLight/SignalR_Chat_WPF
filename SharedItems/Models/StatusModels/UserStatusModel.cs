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
        public int UserModelId { get; set; }
        public virtual BanStatusModel BanStatus { get; set; }
        public virtual MuteStatusModel MuteStatus { get; set; }
    }
}
