namespace PIEAPI.DataLayer.DataTransferObjects
{
    public class Entry
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public DateTime Timestamp { get; set; }
        public int EnteredBy { get; set; }
        public short LocationId { get; set; }
    }
}
