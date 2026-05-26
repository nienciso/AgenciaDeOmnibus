using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistencia
{
    public class Conexion
    {
        private static string _cnn = @"Data source = localhost ; Initial Catalog = PrimerAnio; Integrated security = true ;";

        public static string Cnn
        {
            get { return _cnn; }
        }
    }
}
