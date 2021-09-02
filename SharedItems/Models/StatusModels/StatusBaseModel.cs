using System;

namespace SharedItems.Models.StatusModels
{
    public abstract class StatusBaseModel
    {
        public int Id { get; set; }
        public int UserStatusModelId { get; set; }
        public bool IsPermanent { get; set; }
        public int Duration { get; set; }
        public DateTime EndTime { get; set; }
    }
}