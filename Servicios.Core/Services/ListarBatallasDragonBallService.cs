using AutoMapper;
using Servicios.Core.DTOs;
using Servicios.Core.Interfaces;



namespace Servicios.Core.Services
{
    public class ListarBatallasDragonBallService : IListarBatallasDragonBallService
    {
        private readonly IListarBatallasDragonBallRepository _listarBatallasDragonBallRepository;
        private readonly IMapper _mapper;

        public ListarBatallasDragonBallService(IMapper mapper, IListarBatallasDragonBallRepository listarBatallasDragonBallRepository)
        {
            _listarBatallasDragonBallRepository = listarBatallasDragonBallRepository;
            _mapper = mapper;
        }

        public async Task<PersonajeDragonBallresponseDto> ProgramarBatallas(int limit)
        {
            List<Batallas> batallas = new List<Batallas>();

            Batallas batalla = new Batallas();

            PersonajeDragonBallresponseDto responseDto = new PersonajeDragonBallresponseDto();

            var response = await _listarBatallasDragonBallRepository.ProgramarBatallas(limit);

            int contar = 1;
            int contarbatallas = 0;
            int contarpares = 0;
            if (response.response != "NOK")
            {
                foreach (var personaje in response.items)
                {
                    if (contar == 1)
                    {

                        batalla = new Batallas();
                        batalla.batalla = personaje.name + " vs ";
                        contar++;
                    }
                    else
                    {
                        batalla.batalla = batalla.batalla + personaje.name;
                        DateTime fechaActual = DateTime.Now;  //Fecha y hora actual
                        batalla.fecha = fechaActual.AddDays(30).ToString(); // Add 30 días a Fecha actual
                        batallas.Add(batalla);
                        contar = 1;
                    }
                }

                DateTime fecha2 = DateTime.Now;
                DateTime fecha3 = DateTime.Now;

                foreach (var bat in batallas)
                {
                    contarbatallas++;
                    contarpares++;
                    if (contarpares == 2)
                    {
                        if (contarbatallas > 2)
                        {
                            bat.fecha = fecha3.ToString();
                            fecha2 = Convert.ToDateTime(bat.fecha);
                            contarpares = 0;
                        }
                        else
                        {
                            fecha2 = Convert.ToDateTime(bat.fecha);
                            contarpares = 0;
                        }
                    }
                    else
                    {
                        if (contarbatallas > 2)
                        {

                            bat.fecha = fecha2.AddDays(1).ToString();
                            fecha3 = Convert.ToDateTime(bat.fecha);
                            contarpares = 1;
                        }
                    }
                }

                responseDto.batallas = batallas;

                responseDto = _mapper.Map<PersonajeDragonBallresponseDto>(responseDto);
                responseDto.response = "OK";
            }
            else
            {
                responseDto.error = response.error;
                responseDto.response = "NOK";
            }
            return responseDto;
        }



        public async Task<PersonajeDragonBallresponseDto> ProgramarBatallas2(int limit)
        {
            List<Batallas> batallas = new List<Batallas>();

            Batallas batalla = new Batallas();

            PersonajeDragonBallresponseDto responseDto = new PersonajeDragonBallresponseDto();

            var response = await _listarBatallasDragonBallRepository.ProgramarBatallas(limit);

            int contarbatallas = 0;
            int contarpares = 0;
            if (response.response != "NOK")
            {
                // Convertir los elementos de la respuesta en una lista para acceso por índice
                var personajes = response.items.ToList();


                // Iterar sobre la lista de personajes en pasos de 2 (pares)
                for (int i = 0; i < personajes.Count; i += 2)
                {
                    // Verificar si hay un par completo (evitar índice fuera de rango)
                    if (i + 1 < personajes.Count)
                    {
                        var batalla1 = new Batallas
                        {
                            batalla = $"{personajes[i].name} vs {personajes[i + 1].name}",
                            fecha = DateTime.Now.AddDays(30).ToString()
                        };
                        // Añadir la batalla a la lista de batallas
                        batallas.Add(batalla1);
                    }
                }

                DateTime fecha2 = DateTime.Now;
                DateTime fecha3 = DateTime.Now;

                foreach (var bat in batallas)
                {
                    contarbatallas++;
                    contarpares++;
                    if (contarpares == 2)
                    {
                        if (contarbatallas > 2)
                        {
                            bat.fecha = fecha3.ToString();
                            fecha2 = Convert.ToDateTime(bat.fecha);
                            contarpares = 0;
                        }
                        else
                        {
                            fecha2 = Convert.ToDateTime(bat.fecha);
                            contarpares = 0;
                        }
                    }
                    else
                    {
                        if (contarbatallas > 2)
                        {

                            bat.fecha = fecha2.AddDays(1).ToString();
                            fecha3 = Convert.ToDateTime(bat.fecha);
                            contarpares = 1;
                        }
                    }
                }
                responseDto.batallas = batallas;
                responseDto = _mapper.Map<PersonajeDragonBallresponseDto>(responseDto);
                responseDto.response = "OK";
            }
            else
            {
                responseDto.error = response.error;
                responseDto.response = "NOK";
            }
            return responseDto;
        }
    }
}
