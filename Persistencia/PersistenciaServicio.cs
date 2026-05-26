using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EC;

namespace Persistencia
{
    public class PersistenciaServicio
    {
        public static void AltaServicio(Servicios s)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("AltaServicio", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@NroOmnibus", s.NroOmnibus);
                comando.Parameters.AddWithValue("@FechaHoraP", s.FechaHoraP);
                comando.Parameters.AddWithValue("@FechaHoraLL", s.FechaHoraLL);
                comando.Parameters.AddWithValue("@Precio", s.Precio);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo dar de alta el servicio: ya existe (ómnibus y fecha de partida).");

                    if (r == -2)
                        throw new Exception("No se pudo dar de alta el servicio.");

                    if (r != 1)
                        throw new Exception($"No se pudo dar de alta el servicio. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al dar de alta el servicio.", ex);
                }
            }
        }

        public static void BajaServicio(int nroOmnibus, DateTime fechaHoraP)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("BajaServicio", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@NroOmnibus", nroOmnibus);
                comando.Parameters.AddWithValue("@FechaHoraP", fechaHoraP);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo eliminar el servicio: no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo eliminar el servicio: tiene paquetes asociados.");

                    if (r == -3)
                        throw new Exception("No se pudo eliminar el servicio.");

                    if (r != 1)
                        throw new Exception($"No se pudo eliminar el servicio. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al eliminar el servicio.", ex);
                }
            }
        }

        public static void ModificarServicio(Servicios s)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("ModificarServicio", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@NroOmnibus", s.NroOmnibus);
                comando.Parameters.AddWithValue("@FechaHoraP", s.FechaHoraP);
                comando.Parameters.AddWithValue("@FechaHoraLL", s.FechaHoraLL);
                comando.Parameters.AddWithValue("@Precio", s.Precio);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -1)
                        throw new Exception("No se pudo modificar el servicio: no existe.");

                    if (r == -2)
                        throw new Exception("No se pudo modificar el servicio.");

                    if (r != 1)
                        throw new Exception($"No se pudo modificar el servicio. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al modificar el servicio.", ex);
                }
            }
        }

        public static List<Servicios> ListarServiciosVigentes()
        {
            List<Servicios> lista = new List<Servicios>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("ListarServiciosVigentes", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Servicios s = new Servicios(
                            Convert.ToInt32(lector["NroOmnibus"]),
                            Convert.ToDateTime(lector["FechaHoraP"]),
                            Convert.ToDateTime(lector["FechaHoraLL"]),
                            Convert.ToInt32(lector["Precio"])
                        );

                        lista.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error.", ex);
                }
            }

            return lista;
        }

        public static Servicios BuscarServicio(int nroOmnibus, DateTime fechaHoraP)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("BuscarServicioPorNro", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@NroOmnibus", nroOmnibus);
                comando.Parameters.AddWithValue("@FechaHoraP", fechaHoraP);

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    if (lector.Read())
                    {
                        Servicios s = new Servicios(
                            Convert.ToInt32(lector["NroOmnibus"]),
                            Convert.ToDateTime(lector["FechaHoraP"]),
                            Convert.ToDateTime(lector["FechaHoraLL"]),
                            Convert.ToInt32(lector["Precio"])
                        );

                        return s;
                    }

                    return null; 
                }
                catch (Exception ex)
                {
                    throw new Exception("Error.", ex);
                }
            }
        }
    }
}