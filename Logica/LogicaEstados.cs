using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;
namespace Logica
{
    public class LogicaEstados
    {
        public static void Agregar(Estados estado)
        {
            PersistenciaEstados.AltaEstado(estado);
        }

        public static void Modificar(Estados estado)
        {
             PersistenciaEstados.ModificarEstado(estado);
        }

        public static void Eliminar(string codigo)
        {
             PersistenciaEstados.BajaEstado(codigo);
        }

        public static Estados BuscarEstadoPorCodigo(string codigo)
        {
            return PersistenciaEstados.BuscarEstadoPorCodigo(codigo);
        }

        public static List<Estados> ListarTodos()
        {
            return PersistenciaEstados.BuscarEstados();
        }

    }
}

