namespace AgroControlUI.DTOs.Fertilizers
{
    public class CreateFertilizerWithStringDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? ProducerId { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public ICollection<CreateFertilizerComponentWithStringDto> FertilizerComponents { get; set; } = new List<CreateFertilizerComponentWithStringDto>();
    }
}
