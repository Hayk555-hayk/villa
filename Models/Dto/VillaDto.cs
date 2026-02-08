using System.ComponentModel.DataAnnotations;

namespace villa.Models.Dto
{
    public class VIllaDTO
    {
        public int Id { get; set;}
        [Required]
        [MaxLength(30)]
        public required string Name {get; set;}
    }    
}
