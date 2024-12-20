using EU_CottonSecurity;
using EU_CottonContainer.Model;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Security.Claims;

namespace EU_CottonContainer.Data.Repository
{
    public class AplicacionRepository 
    {
        public List<Aplicacion> GetAllBy(Aplicacion entity)
        {
            //return RolFactory.GetList((DbDataReader)base._ProviderDB.GetDataReader("sp_ObtenRolList", new DbParameter[] { }));
            List<Aplicacion> _lstAplicacion = new List<Aplicacion>();
            Aplicacion _app = new Aplicacion();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenAplicacionUsuario", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idUsuario", MySqlDbType.Int32).Value = entity.idUsuario;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Nombre"] != null)
                        {
                            _app = new Aplicacion
                            {
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Url = dr["Url"].ToString(),
                                icono = dr["icono"].ToString(),
                                Status = int.Parse(dr["Status"].ToString()),
                            };
                            _lstAplicacion.Add(_app);
                        }
                    }
                }
            }
            return _lstAplicacion;
        }

        public int EditApp(Aplicacion entity)
        {
            try
            {
                Aplicacion _App = new Aplicacion();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaAplicacion", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idAplicacion", MySqlDbType.Int32).Value = entity.idAplicacion;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Url", MySqlDbType.String, 120).Value = entity.Url;
                        cmd.Parameters.Add("@icono", MySqlDbType.String, 50).Value = entity.icono;
                        cmd.Parameters.Add("@Status", MySqlDbType.Int32).Value = entity.Status;
                        con.Open();
                        dr = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
                return dr;
            }
            catch
            {
                throw;
            }
        }

        public int AddApp(Aplicacion entity)
        {
            try
            {
                Aplicacion _App = new Aplicacion();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaAplicacion", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Url", MySqlDbType.String, 120).Value = entity.Url;
                        cmd.Parameters.Add("@icono", MySqlDbType.String, 50).Value = entity.icono;
                        con.Open();
                        dr = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
                return dr;
            }
            catch
            {
                throw;
            }
        }

    }
}
