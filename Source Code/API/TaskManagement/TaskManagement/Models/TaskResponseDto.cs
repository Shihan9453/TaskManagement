namespace TaskManagement.Models
{
    public class TaskResponseDto
    {

        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public string Status { get; set; } 
        public string PriorityLevel { get; set; } 
        public string AssignedPerson { get; set; }
        public int AssignedPersonId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Remarks { get; set; }

    }
}
