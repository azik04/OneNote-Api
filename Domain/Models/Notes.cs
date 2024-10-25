using Domain.Models.Base;

namespace Domain.Models;

public class Notes : BaseModel
{
    public string Name { get; set; }
    public string Password { get; set; }
    public long FolderId { get; set; }

    public virtual Folders Folder { get; set; }
}
