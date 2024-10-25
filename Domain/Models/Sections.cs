using Domain.Models.Base;

namespace Domain.Models;

public class Sections : BaseModel
{
    public string Name { get; set; }

    public ICollection<Folders> Folders { get; set; }
}
