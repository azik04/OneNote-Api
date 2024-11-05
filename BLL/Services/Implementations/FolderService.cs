using BLL.Services.Interfaces;
using DAL.Repositories;
using Domain.DTO_s.Folders;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BLL.Services.Implementations;

public class FolderService : IFolderService
{
    private readonly IBaseRepository<Folders> _rep;
    private readonly IBaseRepository<Sections> _srep;


    public FolderService(IBaseRepository<Folders> rep, IBaseRepository<Sections> srep)
    {
        _rep = rep;
        _srep = srep;
    }


    public async Task<IBaseResponse<GetFolderDTO>> Create(FolderDTO vm)
    {
        try
        {
            if (vm.SectionId == 0)
            {
                Log.Warning("Attempted to create folder with invalid Section ID: {SectionId}", vm.SectionId);
                return new BaseResponse<GetFolderDTO>
                {
                    Message = "Section ID cannot be 0. Please provide a valid sectionId ID.",
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                };
            }

            var folderExist = await _srep.GetAll().SingleOrDefaultAsync(x => x.Id == vm.SectionId);

            if (folderExist == null)
            {
                Log.Warning("Section with ID {SectionId} does not exist", vm.SectionId);
                return new BaseResponse<GetFolderDTO>
                {
                    Message = $"Section with ID {vm.SectionId} does not exist. Please provide a valid Section ID.",
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                };
            }

            var data = new Folders()
            {
                CreateAt = DateTime.UtcNow,
                Name = vm.Name,
                SectionId = vm.SectionId
            };

            await _rep.Create(data);

            var dto = new GetFolderDTO()
            {
                Id = data.Id,
                Name = data.Name,
                SectionId = data.SectionId
            };

            Log.Information("Folder {FolderId} created successfully in Section {SectionId}", data.Id, vm.SectionId);

            return new BaseResponse<GetFolderDTO>
            {
                Data = dto,
                Message = "Folder successfully created",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating the folder: {Message}", ex.Message);
            return new BaseResponse<GetFolderDTO>
            {
                Message = "An error occurred while creating the folder: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<ICollection<GetFolderDTO>>> GetAll()
    {
        try
        {
            var data = await _rep.GetAll()
               .Where(x => !x.IsDeleted)
               .Select(item => new GetFolderDTO
               {
                   Id = item.Id,
                   Name = item.Name,
                   SectionId = item.SectionId
               })
               .ToListAsync();

            Log.Information("Successfully retrieved all folders. Count: {FolderCount}", data.Count);

            return new BaseResponse<ICollection<GetFolderDTO>>()
            {
                Data = data,
                Message = "Folders successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the folders: {Message}", ex.Message);
            return new BaseResponse<ICollection<GetFolderDTO>>()
            {
                Message = "An error occurred while retrieving the folders: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<ICollection<GetFolderDTO>>> GetBySection(long sectionId)
    {
        try
        {
            var data = await _rep.GetAll()
               .Where(x => !x.IsDeleted && sectionId == x.SectionId)
               .Select(item => new GetFolderDTO
               {
                   Id = item.Id,
                   Name = item.Name,
                   SectionId = item.SectionId
               })
               .ToListAsync();

            Log.Information("Successfully retrieved folders for Section ID: {SectionId}. Count: {FolderCount}", sectionId, data.Count);

            return new BaseResponse<ICollection<GetFolderDTO>>()
            {
                Data = data,
                Message = "Folders successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving folders for Section ID {SectionId}: {Message}", sectionId, ex.Message);
            return new BaseResponse<ICollection<GetFolderDTO>>()
            {
                Message = "An error occurred while retrieving the folders: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetFolderDTO>> GetById(long id)
    {
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Folder not found for ID: {FolderId}", id);
                return new BaseResponse<GetFolderDTO>
                {
                    Message = "Folder not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var vm = new GetFolderDTO()
            {
                Id = data.Id,
                Name = data.Name,
                SectionId = data.SectionId
            };

            Log.Information("Folder {FolderId} successfully retrieved", data.Id);

            return new BaseResponse<GetFolderDTO>
            {
                Data = vm,
                Message = "Folder successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the folder: {Message}", ex.Message);
            return new BaseResponse<GetFolderDTO>
            {
                Message = "An error occurred while retrieving the folder: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetFolderDTO>> Remove(long id)
    {
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Attempted to remove non-existing folder ID: {FolderId}", id);
                return new BaseResponse<GetFolderDTO>
                {
                    Message = "Folder not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            data.IsDeleted = true;
            data.DeleteAt = DateTime.UtcNow;
            await _rep.Delete(data);

            Log.Information("Folder {FolderId} successfully removed", data.Id);

            return new BaseResponse<GetFolderDTO>
            {
                Data = new GetFolderDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    SectionId = data.SectionId
                },
                Message = "Folder successfully removed",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while removing the folder: {Message}", ex.Message);
            return new BaseResponse<GetFolderDTO>
            {
                Message = "An error occurred while removing the folder: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetFolderDTO>> Update(long Id, FolderDTO vm)
    {
        try
        {
            var item = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == Id && !x.IsDeleted);
            if (item == null)
            {
                Log.Warning("Attempted to update non-existing folder ID: {FolderId}", Id);
                return new BaseResponse<GetFolderDTO>
                {
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                    Message = "Folder not found"
                };
            }

            item.Name = vm.Name;
            item.SectionId = vm.SectionId;

            await _rep.Update(item);

            Log.Information("Folder {FolderId} successfully updated", item.Id);

            return new BaseResponse<GetFolderDTO>
            {
                Data = new GetFolderDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    SectionId = item.SectionId
                },
                Message = "Folder successfully updated",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the folder: {Message}", ex.Message);
            return new BaseResponse<GetFolderDTO>
            {
                Message = "An error occurred while updating the folder: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }
}
