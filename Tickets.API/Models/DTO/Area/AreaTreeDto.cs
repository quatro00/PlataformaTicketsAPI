namespace Tickets.API.Models.DTO.Area
{
    public class AreaTreeDto
    {
        public string value { get; set; }
        public string label { get; set; }
        public bool isLeaf { get; set; }
        public List<AreaTreeDto> children { get; set; }
    }
}
