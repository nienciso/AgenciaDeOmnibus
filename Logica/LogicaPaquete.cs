using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    public class LogicaPaquete
    {
        public static void AltaPaquete(Paquetes paquete)
        {
            PersistenciaPaquete.AltaPaquete(paquete);
        }

        public static List<Paquetes> ListarPaquetes()
        {
            return PersistenciaPaquete.ListarPaquetes();
        }

        public static List<Paquetes> ListarPaquetesPorEstado(Estados estado)
        {
            if (estado == null)
                throw new Exception("Debe proporcionar un estado válido.");

            return PersistenciaPaquete.ListarPaquetesPorEstado(estado);
        }
        public static List<Paquetes> ListarPaquetesPorServicio(Servicios servicio)
        {
            if (servicio == null)
                throw new Exception("Debe proporcionar un servicio válido.");

            return PersistenciaPaquete.ListarPaquetesPorServicio(servicio);
        }

        public static Paquetes BuscarPaquete(int codigo)
        {
            return PersistenciaPaquete.BuscarPaquete(codigo);
        }
    }
}
