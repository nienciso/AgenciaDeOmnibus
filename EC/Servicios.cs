using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
   public class Servicios
    {
        private int nroOmnibus;
        private DateTime fechaHoraP;
        private DateTime fechaHoraLL;
        private int precio;
        public int NroOmnibus
        {
            get { return nroOmnibus; }
            set { nroOmnibus = value; }
        }

        public DateTime FechaHoraP
        {
            get { return fechaHoraP; }
            set
            {
                fechaHoraP = value;
            }
        }

        public string ClaveServicio
        {
            get { return $"{NroOmnibus}|{FechaHoraP}"; }
        }

        public string TextoServicio
        {
            get { return $"Bus {NroOmnibus} - Salida {FechaHoraP:dd/MM/yyyy HH:mm}"; }
        }

        public DateTime FechaHoraLL
        {
            get { return fechaHoraLL; }
            set
            {
                if (value >= FechaHoraP)
                    fechaHoraLL = value;
                else
                    throw new Exception("La fecha de llegada debe ser posterior o igual a la partida.");
            }
        }

        public int Precio
        {
            get { return precio; }
            set
            {
                if (value > 0)
                    precio = value;
                else
                    throw new Exception(" El precio debe ser mayor a 0.");
            }
        }

        public Servicios(int nroOmnibus, DateTime fechaHoraP, DateTime fechaHoraLL, int precio)
        {
            NroOmnibus = nroOmnibus;
            FechaHoraP = fechaHoraP;
            FechaHoraLL = fechaHoraLL;
            Precio = precio;


        }

        public override string ToString()
        {
            return "\n - NroOmnibus : " + NroOmnibus + "\n - FechaHoraP : " + FechaHoraP + "\n - FechaHoraLL : " + FechaHoraLL + "\n - Precio : " + Precio ;
        }

    }
}
