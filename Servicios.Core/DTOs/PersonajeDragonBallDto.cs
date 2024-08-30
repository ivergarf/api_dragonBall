
namespace Servicios.Core.DTOs
{

          
    public class Batallas
    {
        public string batalla { get; set; }
        public string fecha { get; set; }
    }

    public class PersonajeDragonBallresponseDto
    {
        public IList<Batallas> batallas { get; set; }
        public string response { get; set; }
        public string error { get; set; }

    }

}
