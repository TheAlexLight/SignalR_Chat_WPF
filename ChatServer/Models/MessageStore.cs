using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class MessageStore
    {
        public int Id { get; set; }
        public string SavedMessages { get; set; }
    }
}
