using Domain.DTO_s.Notes;
using Domain.Responses;

namespace BLL.Services.Interfaces;

public interface INoteService
{
    public Task<IBaseResponse<GetNoteDTO>> Create(NoteDTO vm);
    public Task<IBaseResponse<ICollection<GetNoteDTO>>> GetAll();
    public Task<IBaseResponse<ICollection<GetNoteDTO>>> GetByFolder(long folderId);
    public Task<IBaseResponse<GetNoteDTO>> GetById(long id);
    public Task<IBaseResponse<GetNoteDTO>> Remove(long id);
    public Task<IBaseResponse<GetNoteDTO>> Update(long Id, NoteDTO vm);
}
