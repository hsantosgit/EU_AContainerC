using EU_CottonContainer.Model;
using EU_CottonSecurity;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class RolRepository
    {
        public List<Rol> GetRol()
        {
            List<Rol> list = new List<Rol>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenRol", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idRol"] != null)
                        {

                            Rol _rol = new Rol
                            {
                                idRol = int.Parse(dr["idRol"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Status = int.Parse(dr["Status"].ToString()),
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString())
                            };
                            list.Add(_rol);
                        }
                    }
                }
                return list;
            }
        }

        public int AddRol(Rol entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaRol", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 45).Value = entity.Nombre;
                        cmd.Parameters.Add("@IdAplicacion", MySqlDbType.Int32).Value = Convert.ToInt32(entity.idAplicacion);
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

        public int DelRolOptions(Rol entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaRolPerfil", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdRol", MySqlDbType.Int32).Value = entity.idRol;
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

        public int AddRolPerfil(Rol entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaRolPerfilNivel", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdRol", MySqlDbType.Int32).Value = entity.idRol;
                        cmd.Parameters.Add("@IdPerfil", MySqlDbType.Int32).Value = Convert.ToInt32(entity.idPerfil);
                        cmd.Parameters.Add("@IdNivel", MySqlDbType.Int32).Value = Convert.ToInt32(entity.idNivel);
                        cmd.Parameters.Add("@UserName", MySqlDbType.String, 45).Value = entity.Nombre;

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

        public int EditRol(Rol entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaRol", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 45).Value = entity.Nombre;
                        cmd.Parameters.Add("@IdRol", MySqlDbType.Int32).Value = Convert.ToInt32(entity.idRol);
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
