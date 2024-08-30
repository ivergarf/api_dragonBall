using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Servicios.Core.Entities;
using Servicios.Core.Interfaces;



namespace Servicios.Infrastructure.Repositories
{
    public class ListarBatallasDragonBallRepository : IListarBatallasDragonBallRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public ListarBatallasDragonBallRepository(IMapper mapper, IConfiguration configuration)
        {
           
            _configuration = configuration;
            _mapper = mapper;
       
        }

        public async Task<PersonajesDragonBallResponseEntity> ProgramarBatallas(int limit)
        {

            PersonajesDragonBallResponseEntity dragonBallResponse = new PersonajesDragonBallResponseEntity();

            if (limit % 2 == 0 && (limit <= 16 && limit > 0))
            {
                try
                {
                    var client = new RestClient(_configuration["AppSettings:url_dragonball"].ToString() + "1&limit=" + limit);
                    var request = new RestRequest("", Method.Get);
                    request.AddHeader("User-Agent", "insomnia/9.3.3");
                    RestResponse response = client.Execute(request);


                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError || response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == 0)
                    {

                        dragonBallResponse.response = "NOK";

                    }
                    else
                    {
                        dragonBallResponse.response = "OK";
                        //var result = JObject.Parse(response.Content);

                        // Deserializar el JSON
                        var root = JsonConvert.DeserializeObject<PersonajesDragonBallResponseEntity>(response.Content);

                        // Revolver el array de personajes
                        var random = new Random();
                        root.items = root.items.OrderBy(x => random.Next()).ToList();

                        dragonBallResponse = _mapper.Map<PersonajesDragonBallResponseEntity>(root);
                    }
                }
                catch (Exception ex)
                {

                    dragonBallResponse.response = "NOK";
                    dragonBallResponse.error = ex.Message;


                }
            }
            else
            {
                dragonBallResponse.response = "NOK";
                dragonBallResponse.error = "El numero de luchadores debe ser par positivo y menor o igual a 16";
            }

           
            return dragonBallResponse;

        }

    }
}
