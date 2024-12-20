using EU_CottonContainer.Model;
using EU_CottonSecurity;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class PerfilRepository
    {
        public List<Perfil> GetPerfil()
        {
            List<Perfil> list = new List<Perfil>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenPerfil", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idPerfil"] != null)
                        {

                            Perfil _perfil = new Perfil
                            {
                                idPerfil = int.Parse(dr["idPerfil"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Status = int.Parse(dr["Status"].ToString()),
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString())
                            };
                            list.Add(_perfil);
                        }
                    }
                }
                return list;
            }
        }

        public List<Perfil> GetPerfilBy(int iRol)
        {
            List<Perfil> list = new List<Perfil>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenPerfilRol", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdRol", MySqlDbType.Int32).Value = iRol;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idPerfil"] != null)
                        {

                            Perfil _perfil = new Perfil
                            {
                                idPerfil = int.Parse(dr["idPerfil"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Status = int.Parse(dr["Status"].ToString()),
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString())
                            };
                            list.Add(_perfil);
                        }
                    }
                }
                return list;
            }
        }

        public int DelUserOptions(Perfil entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaMenuUsuario", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdPerfil", MySqlDbType.Int32).Value = entity.idPerfil;
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

        public int AddPerfilMenu(Perfil entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaMenuPerfil", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdPerfil", MySqlDbType.Int32).Value = entity.idPerfil;
                        cmd.Parameters.Add("@IdMenu", MySqlDbType.Int32).Value = Convert.ToInt32(entity.IdMenu);
                        cmd.Parameters.Add("@UserName", MySqlDbType.String, 45).Value = entity.MenuPadre;
                        
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

        public int AddPerfil(Perfil entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaPerfil", con))
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

        public int EditPerfil(Perfil entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaPerfil", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 45).Value = entity.Nombre;
                        cmd.Parameters.Add("@IdPerfil", MySqlDbType.Int32).Value = Convert.ToInt32(entity.idPerfil);
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
