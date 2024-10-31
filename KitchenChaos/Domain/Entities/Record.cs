namespace Domain.Entities
{
    public class Record : BaseEntity
    {
        public int RecordMark { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
