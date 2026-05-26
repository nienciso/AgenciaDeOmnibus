using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EC;

namespace Persistencia
{
    public class PersistenciaEstados
    {
        public static void AltaEstado(Estados estado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("AltaEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Codigo", estado.Codigo);
                comando.Parameters.AddWithValue("@Nombre", estado.Nombre);
                comando.Parameters.AddWithValue("@Pais", estado.Pais);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo dar de alta el estado: ya existe un estado con ese código.");

                    if (r == -2)
                        throw new Exception("No se pudo dar de alta el estado.");

                    if (r != 1)
                        throw new Exception($"No se pudo dar de alta el estado. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al dar de alta el estado.", ex);
                }
            }
        }

        public static void BajaEstado(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("BajaEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Codigo", codigo);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo eliminar el estado: el estado no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo eliminar el estado: tiene paquetes asociados.");

                    if (r == -3)
                        throw new Exception("No se pudo eliminar el estado: error en la base de datos.");

                    if (r != 1)
                        throw new Exception($"No se pudo eliminar el estado. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al eliminar el estado.", ex);
                }
            }
        }

        public static void ModificarEstado(Estados estado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("ModificarEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Codigo", estado.Codigo);
                comando.Parameters.AddWithValue("@Nombre", estado.Nombre);
                comando.Parameters.AddWithValue("@Pais", estado.Pais);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo modificar el estado: el estado no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo modificar el estado.");

                    if (r != 1)
                        throw new Exception($"No se pudo modificar el estado. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al modificar el estado.", ex);
                }
            }
        }

        public static Estados BuscarEstadoPorCodigo(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("BuscarEstados", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        if (lector["Codigo"].ToString() == codigo)
                        {
                            return new Estados(
                                lector["Codigo"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Pais"].ToString()
                            );
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error.", ex);
                }
            }
        }

        public static List<Estados> BuscarEstados()
        {
            List<Estados> lista = new List<Estados>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("BuscarEstados", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Estados est = new Estados(
                            lector["Codigo"].ToString(),
                            lector["Nombre"].ToString(),
                            lector["Pais"].ToString()
                        );

                        lista.Add(est);
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