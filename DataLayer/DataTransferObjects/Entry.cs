namespace PIEAPI.DataLayer.DataTransferObjects
{
    public class Entry
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime Timestamp { get; set; }
        public int EnteredBy { get; set; }
        public short LocationId { get; set; }
    }
}
