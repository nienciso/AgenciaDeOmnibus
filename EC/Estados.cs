using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class Estados
    {
        private string codigo;
        private string nombre;
        private string pais;

        public string Codigo
        {
            get { return codigo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Length != 4)
                    throw new Exception("El código debe tener exactamente 4 letras.");

                codigo = value.Trim().ToUpper();
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

        public string Pais
        {
            get { return pais; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un pais, no puede estar vacío.");

                pais = value.Trim();
            }
        }

        public Estados(string codigo, string nombre, string pais)
        {
            Codigo = codigo;
            Nombre = nombre;
            Pais = pais;

        }

        public override string ToString()
        {
            return "\n - Codigo : " + Codigo + "\n - Nombre : " + Nombre + "\n - pais : " + Pais;
        }

    }
}
