using AutoMapper;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;
using TicketSystem.Repository;

namespace TicketSystem.Services;

public interface IGarageServices
{
    Task<(Garage? garage, string? error)> Create(GarageForm garageForm);
    Task<(List<GarageDto> garages, int? totalCount, string? error)> GetAll(GarageFilter filter);
    Task<(Garage? garage, string? error)> Update(Guid id, GarageUpdate garageUpdate);
    Task<(Garage? garage, string? error)> Delete(Guid id);
}

public class GarageServices : IGarageServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GarageServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(Garage? garage, string? error)> Create(GarageForm garageForm)
    {
        var garage =_mapper.Map<Garage>(garageForm);
        var response = await _repositoryWrapper.Garage.Add(garage);
        return response == null ? (null, "Error"): (response,null);
    }

    public async Task<(List<GarageDto> garages, int? totalCount, string? error)> GetAll(GarageFilter filter)
    {
        var (data, totalCount) = await _repositoryWrapper.Garage.GetAll<GarageDto>();
        return (_mapper.Map<List<GarageDto>>(data), totalCount, null);
        
    }

    public async Task<(Garage? garage, string? error)> Update(Guid id, GarageUpdate garageUpdate)
    {
        var garage = await _repositoryWrapper.Garage.Get<Garage>(x => x.Id == id);
        if (garage == null) return (null, "Garage not found");
        garage = _mapper.Map(garageUpdate, garage);
        var response = await _repositoryWrapper.Garage.Update(garage);
        return response == null ? (null, "Error"): (response,null);
    }

    public async Task<(Garage? garage, string? error)> Delete(Guid id)
    {
        var garage = await _repositoryWrapper.Garage.Get<Garage>(x => x.Id == id);
        if (garage == null) return (null, "Garage not found");
        var response = await _repositoryWrapper.Garage.Delete(id);
        return response == null ? (null, "Error"): (response,null);
        
    }
}