using System;
using System.Collections.Generic;

namespace Servicios.Core.Entities
{

    public class Items
    {
        public string id { get; set; }
        public string name { get; set; }

    }

    public class PersonajesDragonBallResponseEntity
    {
        public IList<Items> items { get; set; }
        public string response { get; set; }
        public string error { get; set; }

    }


}
