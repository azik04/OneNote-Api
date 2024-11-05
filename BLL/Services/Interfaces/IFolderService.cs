using Domain.DTO_s.Folders;
using Domain.Responses;

namespace BLL.Services.Interfaces;

public interface IFolderService
{
    public Task<IBaseResponse<GetFolderDTO>> Create(FolderDTO vm);
    public Task<IBaseResponse<ICollection<GetFolderDTO>>> GetAll();
    public Task<IBaseResponse<ICollection<GetFolderDTO>>> GetBySection(long sectionId);
    public Task<IBaseResponse<GetFolderDTO>> GetById(long id);
    public Task<IBaseResponse<GetFolderDTO>> Remove(long id);
    public Task<IBaseResponse<GetFolderDTO>> Update(long Id, FolderDTO vm);
}
