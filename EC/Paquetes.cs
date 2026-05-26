using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
   public class Paquetes
    {
        private int codigoP;
        private Hospedajes hospedajes;
        private int cantDias;
        private int precioI;
        private int precioD;
        private int precioT;
        private string descripcion;
        private string titulo;
        private Estados estados;
        private Servicios servicios;
        private int nochesHospedaje;

        public int CodigoP
        {
            get { return codigoP; }
            private set { codigoP = value; }
        }
        public Hospedajes Hospedajes
        {
            get { return hospedajes; }
            set
            {
                if (value != null)
                    hospedajes = value;
                else
                    throw new Exception("El empleado no puede ser nulo.");
            }
        }

        public int CantDias
        {
            get { return cantDias; }
            set
            {
                if (value > 0)
                    cantDias = value;
                else
                    throw new Exception("la cantiadad de dias debe ser mayor a 0.");
            }
        }

        public int PrecioI
        {
            get { return precioI; }
            set
            {
                if (value > 0)
                    precioI = value;
                else
                    throw new Exception("El precio individual debe ser mayor a 0.");
            }
        }

        public int PrecioD
        {
            get { return precioD; }
            set
            {
                if (value > 0)
                    precioD = value;
                else
                    throw new Exception("El precio doble debe ser mayor a 0.");
            }
        }

        public int PrecioT
        {
            get { return precioT; }
            set
            {
                if (value > 0)
                    precioT = value;
                else
                    throw new Exception("El precio triple debe ser mayor a 0.");
            }
        }

        public string Titulo
        {
            get { return titulo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un titulo, no puede estar vacío.");

                titulo = value.Trim();
            }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese una descripcion, no puede estar vacío.");

                descripcion = value.Trim();
            }
        }

        public Estados Estados
        {
            get { return estados; }
            set
            {
                if (value != null)
                    estados = value;
                else
                    throw new Exception("El estado no puede ser nulo.");
            }
        }

        public Servicios Servicios
        {
            get { return servicios; }
            set
            {
                if (value != null)
                    servicios = value;
                else
                    throw new Exception("El servicio no puede ser nulo.");
            }
        }

        public int NochesHospedaje
        {
            get { return nochesHospedaje; }
            set
            {
                if (value <= 0)
                    throw new Exception("Las noches deben ser mayor a 0.");
                nochesHospedaje = value;
            }
        }

        public Paquetes(
            string titulo,
            string descripcion,
            Estados estado,
            int cantDias,
            int precioIndividual,
            int precioDoble,
            int precioTriple,
            Servicios servicio,
            Hospedajes hospedaje,
            int nochesHospedaje)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Estados = estado;
            CantDias = cantDias;
            PrecioI = precioIndividual;
            PrecioD = precioDoble;
            PrecioT = precioTriple;
            Servicios = servicio;
            Hospedajes = hospedaje;
            NochesHospedaje = nochesHospedaje;
        }

        public override string ToString()
        {
            return
                $"\nCódigo: {CodigoP}" +
                $"\nTítulo: {Titulo}" +
                $"\nDescripción: {Descripcion}" +
                $"\nEstado: {Estados.Nombre}" +
                $"\nDías: {CantDias}" +
                $"\nNoches: {NochesHospedaje}" +
                $"\nPrecio Individual: {PrecioI}" +
                $"\nPrecio Doble: {PrecioD}" +
                $"\nPrecio Triple: {PrecioT}" +
                $"\nServicio: {Servicios.NroOmnibus}" +
                $"\nHospedaje: {Hospedajes.Nombre}";
        }
    }
}
