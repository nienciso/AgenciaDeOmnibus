using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EC;

namespace Persistencia
{
    public class PersistenciaPaquete
    {
        public static void AltaPaquete(Paquetes p)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("AltaPaquete", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Titulo", p.Titulo);
                comando.Parameters.AddWithValue("@Descripcion", p.Descripcion);
                comando.Parameters.AddWithValue("@EstadoDestino", p.Estados.Codigo);
                comando.Parameters.AddWithValue("@CantidadDias", p.CantDias);
                comando.Parameters.AddWithValue("@PrecioIndividual", p.PrecioI);
                comando.Parameters.AddWithValue("@PrecioBaseDoble", p.PrecioD);
                comando.Parameters.AddWithValue("@PrecioBaseTriple", p.PrecioT);
                comando.Parameters.AddWithValue("@ServicioCodigo", p.Servicios.NroOmnibus);
                comando.Parameters.AddWithValue("@FechaPartida", p.Servicios.FechaHoraP);
                comando.Parameters.AddWithValue("@HospedajeCodigo", p.Hospedajes.CodigoInterno);
                comando.Parameters.AddWithValue("@NochesHospedaje", p.NochesHospedaje);

                SqlParameter retorno = new SqlParameter("@ReturnVal", SqlDbType.Int)
                { Direction = ParameterDirection.ReturnValue };
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = (int)retorno.Value;

                    if (r == -10) throw new Exception("No se pudo dar de alta el paquete: el título está vacío.");
                    if (r == -11) throw new Exception("No se pudo dar de alta el paquete: la descripción está vacía.");
                    if (r == -12) throw new Exception("No se pudo dar de alta el paquete: noches de hospedaje inválidas.");
                    if (r == -13) throw new Exception("No se pudo dar de alta el paquete: servicio inválido.");

                    if (r == -1) throw new Exception("No se pudo dar de alta el paquete: el estado destino no existe.");
                    if (r == -2) throw new Exception("No se pudo dar de alta el paquete: el servicio (ómnibus/fecha) no existe.");
                    if (r == -3) throw new Exception("No se pudo dar de alta el paquete: el hospedaje no existe.");
                    if (r == -5) throw new Exception("No se pudo dar de alta el paquete: el hospedaje no pertenece al estado destino.");

                    if (r == -6) throw new Exception("No se pudo dar de alta el paquete: la cantidad de días calculada es inválida.");
                    if (r == -7) throw new Exception("No se pudo dar de alta el paquete: noches de hospedaje no pueden ser >= a los días del viaje.");
                    if (r == -8) throw new Exception("No se pudo dar de alta el paquete: no se pudieron obtener precios de servicio/hospedaje.");

                    if (r == -4) throw new Exception("No se pudo dar de alta el paquete.");

                    if (r != 1)
                        throw new Exception($"No se pudo dar de alta el paquete. Código: {r}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error de base de datos al dar de alta el paquete.", ex);
                }
            }
        }

        public static List<Paquetes> ListarPaquetes()
        {
            List<Paquetes> lista = new List<Paquetes>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("ListarPaquetes", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Estados est = PersistenciaEstados.BuscarEstadoPorCodigo(
                            lector["EstadoDestino"].ToString());

                        Hospedajes hos = PersistenciaHospedaje.BuscarHospedaje(
                            lector["HospedajeCodigo"].ToString());

                        int nro = Convert.ToInt32(lector["ServicioCodigo"]);
                        DateTime fecha = Convert.ToDateTime(lector["FechaPartida"]);

                        Servicios ser = PersistenciaServicio.BuscarServicio(nro, fecha);

                        Paquetes p = new Paquetes(
                            lector["Titulo"].ToString(),
                            lector["Descripcion"].ToString(),
                            est,
                            Convert.ToInt32(lector["CantidadDias"]),
                            Convert.ToInt32(lector["PrecioIndividual"]),
                            Convert.ToInt32(lector["PrecioBaseDoble"]),
                            Convert.ToInt32(lector["PrecioBaseTriple"]),
                            ser,
                            hos,
                            Convert.ToInt32(lector["NochesHospedaje"])
                        );

                        typeof(Paquetes)
                            .GetProperty("CodigoP")
                            .SetValue(p, Convert.ToInt32(lector["CodigoPaquete"]));

                        lista.Add(p);
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Error.");
                }
            }

            return lista;
        }

        public static List<Paquetes> ListarPaquetesPorEstado(Estados estado)
        {
            List<Paquetes> lista = new List<Paquetes>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("ListarPaquetesPorEstado", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoEstado", estado.Codigo);

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Hospedajes hos = PersistenciaHospedaje.BuscarHospedaje(
                            lector["HospedajeCodigo"].ToString());

                        int nro = Convert.ToInt32(lector["ServicioCodigo"]);
                        DateTime fecha = Convert.ToDateTime(lector["FechaPartida"]);
                        Servicios ser = PersistenciaServicio.BuscarServicio(nro, fecha);

                        Paquetes p = new Paquetes(
                            lector["Titulo"].ToString(),
                            lector["Descripcion"].ToString(),
                            estado,
                            Convert.ToInt32(lector["CantidadDias"]),
                            Convert.ToInt32(lector["PrecioIndividual"]),
                            Convert.ToInt32(lector["PrecioBaseDoble"]),
                            Convert.ToInt32(lector["PrecioBaseTriple"]),
                            ser,
                            hos,
                            Convert.ToInt32(lector["NochesHospedaje"])
                        );

                        typeof(Paquetes).GetProperty("CodigoP")
                            .SetValue(p, Convert.ToInt32(lector["CodigoPaquete"]));

                        lista.Add(p);
                    }
                }
            }

            return lista;
        }

        public static List<Paquetes> ListarPaquetesPorServicio(Servicios servicio)
        {
            List<Paquetes> lista = new List<Paquetes>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("ListarPaquetesPorServicio", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@ServicioCodigo", servicio.NroOmnibus);
                comando.Parameters.AddWithValue("@FechaPartida", servicio.FechaHoraP);

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Estados est = PersistenciaEstados.BuscarEstadoPorCodigo(
                            lector["EstadoDestino"].ToString());

                        Hospedajes hos = PersistenciaHospedaje.BuscarHospedaje(
                            lector["HospedajeCodigo"].ToString());

                        Paquetes p = new Paquetes(
                            lector["Titulo"].ToString(),
                            lector["Descripcion"].ToString(),
                            est,
                            Convert.ToInt32(lector["CantidadDias"]),
                            Convert.ToInt32(lector["PrecioIndividual"]),
                            Convert.ToInt32(lector["PrecioBaseDoble"]),
                            Convert.ToInt32(lector["PrecioBaseTriple"]),
                            servicio, 
                            hos,
                            Convert.ToInt32(lector["NochesHospedaje"])
                        );

                        typeof(Paquetes).GetProperty("CodigoP")
                            .SetValue(p, Convert.ToInt32(lector["CodigoPaquete"]));

                        lista.Add(p);
                    }
                }
            }

            return lista;
        }

        public static Paquetes BuscarPaquete(int codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                SqlCommand comando = new SqlCommand("BuscarPaquetes", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoPaquete", codigo);

                try
                {
                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    if (lector.Read())
                    {
                        Estados est = PersistenciaEstados.BuscarEstadoPorCodigo(
                            lector["EstadoDestino"].ToString());

                        Hospedajes hos = PersistenciaHospedaje.BuscarHospedaje(
                            lector["HospedajeCodigo"].ToString());

                        int nro = Convert.ToInt32(lector["ServicioCodigo"]);
                        DateTime fecha = Convert.ToDateTime(lector["FechaPartida"]);

                        Servicios ser = PersistenciaServicio.BuscarServicio(nro, fecha);

                        Paquetes p = new Paquetes(
                            lector["Titulo"].ToString(),
                            lector["Descripcion"].ToString(),
                            est,
                            Convert.ToInt32(lector["CantidadDias"]),
                            Convert.ToInt32(lector["PrecioIndividual"]),
                            Convert.ToInt32(lector["PrecioBaseDoble"]),
                            Convert.ToInt32(lector["PrecioBaseTriple"]),
                            ser,
                            hos,
                            Convert.ToInt32(lector["NochesHospedaje"])
                        );

                        typeof(Paquetes)
                            .GetProperty("CodigoP")
                            .SetValue(p, Convert.ToInt32(lector["CodigoPaquete"]));

                        return p;
                    }

                    return null;
                }
                catch (Exception)
                {
                    throw new Exception("Error.");
                }
            }
        }
    }
}