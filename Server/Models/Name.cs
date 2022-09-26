using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W1.Server.Models;

[Table("W1_Name")]
public class W1Name
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public string Name { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime UpdateTime { get; set; }

}
