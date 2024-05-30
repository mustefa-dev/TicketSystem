
using AutoMapper;
using TicketSystem.Repository;
using TicketSystem.Services;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;

namespace TicketSystem.Services;


public interface ISolverDataServices
{
Task<(SolverData? solverdata, string? error)> Create(SolverDataForm solverdataForm );
Task<(List<SolverDataDto> solverdatas, int? totalCount, string? error)> GetAll(SolverDataFilter filter);
Task<(SolverData? solverdata, string? error)> Update(Guid id , SolverDataUpdate solverdataUpdate);
Task<(SolverData? solverdata, string? error)> Delete(Guid id);
}

public class SolverDataServices : ISolverDataServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public SolverDataServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(SolverData? solverdata, string? error)> Create(SolverDataForm solverdataForm )
{
    throw new NotImplementedException();
      
}

public async Task<(List<SolverDataDto> solverdatas, int? totalCount, string? error)> GetAll(SolverDataFilter filter)
    {
        throw new NotImplementedException();
    }

public async Task<(SolverData? solverdata, string? error)> Update(Guid id ,SolverDataUpdate solverdataUpdate)
    {
        throw new NotImplementedException();
      
    }

public async Task<(SolverData? solverdata, string? error)> Delete(Guid id)
    {
        throw new NotImplementedException();
   
    }

}
