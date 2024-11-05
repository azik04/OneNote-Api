using BLL.Services.Interfaces;
using DAL.Repositories;
using Domain.DTO_s.Sections;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BLL.Services.Implementations;

public class SectionService : ISectionService
{
    private readonly IBaseRepository<Sections> _rep;


    public SectionService(IBaseRepository<Sections> rep)
    {
        _rep = rep;
    }


    public async Task<IBaseResponse<GetSectionDTO>> Create(SectionDTO vm)
    {
        Log.Information("Creating a new section with name: {SectionName}", vm.Name);
        try
        {
            var data = new Sections()
            {
                CreateAt = DateTime.UtcNow,
                Name = vm.Name,
            };

            await _rep.Create(data);

            var dto = new GetSectionDTO()
            {
                Id = data.Id,
                Name = data.Name,
            };

            Log.Information("Section created successfully with ID: {SectionId}", data.Id);
            return new BaseResponse<GetSectionDTO>
            {
                Data = dto,
                Message = "Section successfully created",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating the section: {ErrorMessage}", ex.Message);
            return new BaseResponse<GetSectionDTO>
            {
                Message = "An error occurred while creating the section: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<ICollection<GetSectionDTO>>> GetAll()
    {
        Log.Information("Retrieving all sections");
        try
        {
            var data = await _rep.GetAll()
                .Where(x => !x.IsDeleted)
                .Select(item => new GetSectionDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                })
                .ToListAsync();

            Log.Information("Successfully retrieved {Count} sections", data.Count);
            return new BaseResponse<ICollection<GetSectionDTO>>()
            {
                Data = data,
                Message = "Sections successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the sections: {ErrorMessage}", ex.Message);
            return new BaseResponse<ICollection<GetSectionDTO>>()
            {
                Message = "An error occurred while retrieving the sections: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetSectionDTO>> GetById(long id)
    {
        Log.Information("Retrieving section with ID: {SectionId}", id);
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Section with ID: {SectionId} not found", id);
                return new BaseResponse<GetSectionDTO>
                {
                    Message = "Section not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var dto = new GetSectionDTO()
            {
                Id = data.Id,
                Name = data.Name,
            };

            Log.Information("Section with ID: {SectionId} retrieved successfully", id);
            return new BaseResponse<GetSectionDTO>
            {
                Data = dto,
                Message = "Section successfully retrieved",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving the section: {ErrorMessage}", ex.Message);
            return new BaseResponse<GetSectionDTO>
            {
                Message = "An error occurred while retrieving the section: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetSectionDTO>> Remove(long id)
    {
        Log.Information("Removing section with ID: {SectionId}", id);
        try
        {
            var data = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
            {
                Log.Warning("Section with ID: {SectionId} not found", id);
                return new BaseResponse<GetSectionDTO>
                {
                    Message = "Section not found",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            data.IsDeleted = true;
            data.DeleteAt = DateTime.UtcNow;
            await _rep.Delete(data);
            var dto = new GetSectionDTO()
            {
                Id = data.Id,
                Name = data.Name,
            };

            Log.Information("Section with ID: {SectionId} successfully removed", id);
            return new BaseResponse<GetSectionDTO>
            {
                Data = dto,
                Message = "Section successfully removed",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while removing the section: {ErrorMessage}", ex.Message);
            return new BaseResponse<GetSectionDTO>
            {
                Message = "An error occurred while removing the section: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }


    public async Task<IBaseResponse<GetSectionDTO>> Update(long id, SectionDTO vm)
    {
        Log.Information("Updating section with ID: {SectionId}", id);
        try
        {
            var item = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (item == null)
            {
                Log.Warning("Section with ID: {SectionId} not found", id);
                return new BaseResponse<GetSectionDTO>
                {
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                    Message = "Section not found"
                };
            }
            item.Name = vm.Name;

            await _rep.Update(item);

            var dto = new GetSectionDTO()
            {
                Id = item.Id,
                Name = item.Name,
            };

            Log.Information("Section with ID: {SectionId} successfully updated", id);
            return new BaseResponse<GetSectionDTO>
            {
                Data = dto,
                Message = "Section successfully updated",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the section: {ErrorMessage}", ex.Message);
            return new BaseResponse<GetSectionDTO>
            {
                Message = "An error occurred while updating the section: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }
}
