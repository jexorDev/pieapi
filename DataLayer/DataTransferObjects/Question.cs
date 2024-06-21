namespace PIEAPI.DataLayer.DataTransferObjects
{
    public class Question
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Text { get; set; }
        public int? CreatedBy { get; set; }
    }
}
