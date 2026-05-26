using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    public class LogicaServicios
    {
        public static void Agregar(Servicios s)
        {
            PersistenciaServicio.AltaServicio(s);
        }

        public static void Eliminar(int nroOmnibus, DateTime fechaHoraP)
        {
             PersistenciaServicio.BajaServicio(nroOmnibus, fechaHoraP);
        }

        public static void Modificar(Servicios s)
        {
            PersistenciaServicio.ModificarServicio(s);
        }

        public static Servicios BuscarServicio(int nroOmnibus, DateTime fechaHoraP)
        {
            return PersistenciaServicio.BuscarServicio(nroOmnibus, fechaHoraP);
        }

        public static List<Servicios> ListarServiciosVigentes()
        {
            return PersistenciaServicio.ListarServiciosVigentes();
        }
    }
}
