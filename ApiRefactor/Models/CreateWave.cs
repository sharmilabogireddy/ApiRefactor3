using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.Models
{
    public class CreateWave
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(1, ErrorMessage = "Name cannot be empty.")]
        public string Name { get; set; }

    }
}
