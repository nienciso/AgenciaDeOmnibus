using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EC;

namespace Persistencia
{
    public class PersistenciaHospedaje
    {
        public static void AltaHospedaje(Hospedajes h)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("AltaHospedaje", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoInterno", h.CodigoInterno);
                comando.Parameters.AddWithValue("@Nombre", h.Nombre);
                comando.Parameters.AddWithValue("@Direccion", h.Direccion);
                comando.Parameters.AddWithValue("@TipoHospedaje", h.TipoH);
                comando.Parameters.AddWithValue("@Precio", h.Precio);
                comando.Parameters.AddWithValue("@Estado", h.Estados.Codigo);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo dar de alta el hospedaje: ya existe ese código interno.");

                    if (r == -2)
                        throw new Exception("No se pudo dar de alta el hospedaje.");

                    if (r != 1)
                        throw new Exception($"No se pudo dar de alta el hospedaje. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al dar de alta el hospedaje.", ex);
                }
            }
        }

        public static void BajaHospedaje(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("BajaHospedaje", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoInterno", codigo);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo eliminar el hospedaje: no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo eliminar el hospedaje: tiene paquetes asociados.");

                    if (r == -3)
                        throw new Exception("No se pudo eliminar el hospedaje.");

                    if (r != 1)
                        throw new Exception($"No se pudo eliminar el hospedaje. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al eliminar el hospedaje.", ex);
                }
            }
        }

        public static void ModificarHospedaje(Hospedajes h)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("ModificarHospedaje", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoInterno", h.CodigoInterno);
                comando.Parameters.AddWithValue("@Nombre", h.Nombre);
                comando.Parameters.AddWithValue("@Direccion", h.Direccion);
                comando.Parameters.AddWithValue("@TipoHospedaje", h.TipoH);
                comando.Parameters.AddWithValue("@Precio", h.Precio);
                comando.Parameters.AddWithValue("@Estado", h.Estados.Codigo);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo modificar el hospedaje: no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo modificar el hospedaje.");

                    if (r != 1)
                        throw new Exception($"No se pudo modificar el hospedaje. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al modificar el hospedaje.", ex);
                }
            }
        }

        public static Hospedajes BuscarHospedaje(string codigo)
        {
            Hospedajes h = null;

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("BuscarHospedajePorCodigo", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoInterno", codigo);

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    if (lector.Read())
                    {
                        string codEstado = lector["Estado"].ToString();
                        Estados est = PersistenciaEstados.BuscarEstadoPorCodigo(codEstado);

                        h = new Hospedajes(
                            lector["CodigoInterno"].ToString(),
                            lector["TipoHospedaje"].ToString(),
                            lector["Nombre"].ToString(),
                            lector["Direccion"].ToString(),
                            Convert.ToInt32(lector["Precio"]),
                            est
                        );
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener hospedajes por estado: " + ex.Message);
                }
            }

            return h;
        }

        public static List<Hospedajes> ObtenerHospedajesPorEstado(Estados estado)
        {
            List<Hospedajes> lista = new List<Hospedajes>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("ObtenerHospedajesPorEstado", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoEstado", estado.Codigo);

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Hospedajes h = new Hospedajes(
                            lector["CodigoInterno"].ToString(),
                            lector["TipoHospedaje"].ToString(),
                            lector["Nombre"].ToString(),
                            lector["Direccion"].ToString(),
                            Convert.ToInt32(lector["Precio"]),
                            estado
                        );

                        lista.Add(h);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error.", ex);
                }
            }

            return lista;
        }
    }
}