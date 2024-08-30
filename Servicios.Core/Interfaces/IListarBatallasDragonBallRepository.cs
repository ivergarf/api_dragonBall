

using Servicios.Core.Entities;

namespace Servicios.Core.Interfaces
{
    public interface IListarBatallasDragonBallRepository
    {
        Task<PersonajesDragonBallResponseEntity> ProgramarBatallas(int limit);
    }
}
