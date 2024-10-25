using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Base;

public class BaseModel
{
    [Key]
    public long Id { get; set; }
    public DateTime DeleteAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreateAt { get; set; }
}
