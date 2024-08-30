
using Servicios.Core.DTOs;


namespace Servicios.Core.Interfaces
{
    public interface IListarBatallasDragonBallService
    {
        Task<PersonajeDragonBallresponseDto> ProgramarBatallas(int limit);
    }
}
