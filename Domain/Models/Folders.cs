using Domain.Models.Base;

namespace Domain.Models;

public class Folders : BaseModel
{
    public string Name { get; set; }
    public long SectionId { get; set; }

    public Sections Section { get; set; }
    public ICollection<Notes> Notes { get; set; }

}
