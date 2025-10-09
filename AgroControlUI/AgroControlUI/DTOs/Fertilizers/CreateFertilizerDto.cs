namespace AgroControlUI.DTOs.Fertilizers
{
    public class CreateFertilizerDto
    {
            public int? Id { get; set; }
            public  string? Name { get; set; }
            public int? ProducerId { get; set; }
            public  int? CategoryId { get; set; }
            public string? Description { get; set; }
            public ICollection<CreateFertilizerComponentDto> FertilizerComponents { get; set; } = new List<CreateFertilizerComponentDto>();

    }
}
