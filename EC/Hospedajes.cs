using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
   public class Hospedajes
    {
        private string codigoInterno;
        private string tipoH;
        private string direccion;
        private int precio;
        private string nombre;
        private Estados estadoProp;

        public string CodigoInterno
        {
            get { return codigoInterno; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Debe ingresar un código interno.");

                string cod = value.Trim().ToUpper();

                if (cod.Length < 1 || cod.Length > 10)
                    throw new Exception("El Código Interno debe tener entre 1 y 10 letras.");

                foreach (char c in cod)
                {
                    if (!char.IsLetter(c))
                        throw new Exception("El Código Interno solo puede contener letras.");
                }

                codigoInterno = cod;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un nombre, no puede estar vacío.");

                nombre = value.Trim();
            }
        }

        public string Direccion
        {
            get { return direccion; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    direccion = value;
                else
                    throw new Exception("La dirección no puede estar vacía.");
            }
        }

        public Estados Estados
        {
            get { return estadoProp; }
            set
            {
                if (value != null)
                    estadoProp = value;
                else
                    throw new Exception("El estado no puede ser nulo.");
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

        public string TipoH
        {
            get { return tipoH; }
            set
            {
                if (value == null)
                    throw new Exception("Debe ingresar un tipo de hospedaje.");

                string limpio = value.Trim().Replace("  ", " ");

                string[] permitidos = { "Hotel STD", "Posada", "All Inclusive" };

                if (permitidos.Contains(limpio, StringComparer.OrdinalIgnoreCase))
                {
                    tipoH = limpio;
                }
                else
                {
                    throw new Exception("Tipo de Hospedaje no válido. Debe ser 'Hotel STD', 'Posada' o 'All Inclusive'.");
                }
            }
        }

        public string EstadoNombre
        {
            get { return Estados.Nombre; }
        }

        public Hospedajes(string codigoInterno,string tipoH ,string nombre, string direccion, int precio, Estados estadoProp)
        {
            CodigoInterno = codigoInterno;
            TipoH = tipoH;
            Nombre = nombre;
            Direccion = direccion;
            Precio = precio;
            Estados = estadoProp;

        }



        public override string ToString()
        {
            return "\n - CodigoInterno : " + CodigoInterno + "\n - Nombre : " + Nombre + "\n - Direccion : " + Direccion + "\n - Precio : " + Precio + "\n - Estados : " + Estados + "\n - Tipo Hospedaje : " + TipoH;
        }
    }
}
