namespace PIEAPI.DataLayer.DataTransferObjects
{
    public class StatisticQuestionCount
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }
}
