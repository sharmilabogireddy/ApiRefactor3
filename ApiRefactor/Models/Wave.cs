using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.Models;

public class Wave
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    public DateTime WaveDate { get; set; }

}
