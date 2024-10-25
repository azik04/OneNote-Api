using BLL.Services.Interfaces;
using DAL.Repositories;
using Domain.DTO_s.Notes;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BLL.Services.Implementations;

public class NoteService : INoteService
{
    private readonly IBaseRepository<Notes> _rep;
    private readonly IBaseRepository<Folders> _frep;

    public NoteService(IBaseRepository<Notes> rep, IBaseRepository<Folders> frep)
    {
        _rep = rep;
        _frep = frep;
    }

    public async Task<IBaseResponse<GetNoteDTO>> Create(NoteDTO vm)
    {
        try
        {
            if (vm.FolderId == 0)
            {
                Log.Warning("Folder ID cannot be 0. Invalid input provided.");
                return new BaseResponse<GetNoteDTO>
                {
                    Message = "Folder ID cannot be 0. Please provide a valid folder ID.",
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                };
            }

            var folderExist = await _frep.GetAll().SingleOrDefaultAsync(x => x.Id == vm.FolderId);

            if (folderExist == null)
            {
                Log.Warning($"Folder with ID {vm.FolderId} does not exist.");
                return new BaseResponse<GetNoteDTO>
                {
                    Message = $"Folder with ID {vm.FolderId} does not exist. Please provide a valid folder ID.",
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                };
            }

            var data = new Notes()
            {
                CreateAt = DateTime.UtcNow,
                Name = vm.Name,
                FolderId = vm.FolderId,
                Password = vm.Password,
            };

            await _rep.Create(data);

            var dto = new GetNoteDTO()
            {
                Name = data.Name,
                Password = data.Password
            };

            Log.Information("Note successfully created with ID {NoteId}", data.Id);
            return new BaseResponse<GetNoteDTO>
            {
                Data = dto,
                Message = "Note successfully created",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating the note.");
            return new BaseResponse<GetNoteDTO>
            {
                Message = "An error occurred while creating the note: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<ICollection<GetNoteDTO>>> GetAll()
    {
        try
        {
            var data = await _rep.GetAll()
               .Where(x => !x.IsDeleted)
               .Select(item => new GetNoteDTO
               {
                   Id = item.Id,
                   Name = item.Name,
                   Password = item.Password,
               })
               .ToListAsync();

            Log.Information("Notes successfully retrieved. Count: {Count}", data.Count);
            return new BaseResponse<ICollection<GetNoteDTO>>()
            {
                Data = data,
                Message = "Notes successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the notes.");
            return new BaseResponse<ICollection<GetNoteDTO>>()
            {
                Message = "An error occurred while retrieving the notes: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<ICollection<GetNoteDTO>>> GetByFolder(long folderId)
    {
        try
        {
            var data = await _rep.GetAll()
               .Where(x => !x.IsDeleted && folderId == x.FolderId)
               .Select(item => new GetNoteDTO
               {
                   Id = item.Id,
                   Name = item.Name,
                   Password = item.Password,
               })
               .ToListAsync();

            Log.Information("Notes successfully retrieved for Folder ID: {FolderId}. Count: {Count}", folderId, data.Count);
            return new BaseResponse<ICollection<GetNoteDTO>>()
            {
                Data = data,
                Message = "Notes successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the notes for Folder ID: {FolderId}", folderId);
            return new BaseResponse<ICollection<GetNoteDTO>>()
            {
                Message = "An error occurred while retrieving the notes: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetNoteDTO>> GetById(long id)
    {
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Note not found with ID: {NoteId}", id);
                return new BaseResponse<GetNoteDTO>
                {
                    Message = "Note not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var vm = new GetNoteDTO()
            {
                Name = data.Name,
                Password = data.Password,
                Id = data.Id
            };

            Log.Information("Note successfully retrieved with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>()
            {
                Data = vm,
                Message = "Note successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the note with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>
            {
                Message = "An error occurred while retrieving the note: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetNoteDTO>> Remove(long id)
    {
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Note not found with ID: {NoteId}", id);
                return new BaseResponse<GetNoteDTO>
                {
                    Message = "Note not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            data.DeleteAt = DateTime.UtcNow;
            data.IsDeleted = true;

            var vm = new GetNoteDTO()
            {
                Name = data.Name,
                Password = data.Password,
                Id = data.Id
            };

            Log.Information("Note successfully removed with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>()
            {
                Data = vm,
                Message = "Note successfully removed",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while removing the note with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>
            {
                Message = "An error occurred while removing the note: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetNoteDTO>> Update(long id, NoteDTO vm)
    {
        try
        {
            var item = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (item == null)
            {
                Log.Warning("Note not found for update with ID: {NoteId}", id);
                return new BaseResponse<GetNoteDTO>
                {
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                    Message = "Note not found"
                };
            }

            item.Name = vm.Name;
            item.Password = vm.Password;

            await _rep.Update(item);

            var dto = new GetNoteDTO()
            {
                Name = item.Name,
                Password = item.Password,
                Id = item.Id
            };

            Log.Information("Note successfully updated with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>
            {
                Data = dto,
                Message = "Note successfully updated",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the note with ID: {NoteId}", id);
            return new BaseResponse<GetNoteDTO>
            {
                Message = "An error occurred while updating the note: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }
}
