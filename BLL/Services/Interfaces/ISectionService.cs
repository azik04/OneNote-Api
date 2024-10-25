using Domain.DTO_s.Folders;
using Domain.DTO_s.Sections;
using Domain.Responses;

namespace BLL.Services.Interfaces;

public interface ISectionService
{
    public Task<IBaseResponse<GetSectionDTO>> Create(SectionDTO vm);
    public Task<IBaseResponse<ICollection<GetSectionDTO>>> GetAll();
    public Task<IBaseResponse<GetSectionDTO>> GetById(long id);
    public Task<IBaseResponse<GetSectionDTO>> Remove(long id);
    public Task<IBaseResponse<GetSectionDTO>> Update(long Id, SectionDTO vm);
}
