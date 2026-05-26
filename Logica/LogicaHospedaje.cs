using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    public class LogicaHospedaje
    {
        public static void Agregar(Hospedajes hospedaje)
        {
             PersistenciaHospedaje.AltaHospedaje(hospedaje);
        }

        public static void Modificar(Hospedajes hospedaje)
        {
             PersistenciaHospedaje.ModificarHospedaje(hospedaje);
        }

        public static void Eliminar(string codigoInterno)
        {
             PersistenciaHospedaje.BajaHospedaje(codigoInterno);
        }

        public static Hospedajes BuscarHospedaje(string codigoInterno)
        {
            return PersistenciaHospedaje.BuscarHospedaje(codigoInterno);
        }

        public static List<Hospedajes> ObtenerHospedajePorEstado(Estados estado)
        {
            return PersistenciaHospedaje.ObtenerHospedajesPorEstado(estado);
        }
    }
}
