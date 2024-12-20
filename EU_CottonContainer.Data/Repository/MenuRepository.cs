using EU_CottonSecurity;
using EU_CottonContainer.Model;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class MenuRepository  
    {
        public int Add(Menu entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaMenu", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Icon", MySqlDbType.String, 50).Value = entity.Icon;
                        cmd.Parameters.Add("@IdAplicacion", MySqlDbType.Int32).Value = entity.idAplicacion;
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

        public int Edit(Menu entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaMenu", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMenu", MySqlDbType.Int32).Value = entity.idMenu;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Icon", MySqlDbType.String, 50).Value = entity.Icon;
                        
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

        public int AddSubMenu(Menu entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaSubMenu", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Url", MySqlDbType.String, 50).Value = entity.Icon;
                        cmd.Parameters.Add("@IdAplicacion", MySqlDbType.Int32).Value = entity.idAplicacion;
                        cmd.Parameters.Add("@Orden", MySqlDbType.Int32).Value = entity.Orden;
                        cmd.Parameters.Add("@IdPadre", MySqlDbType.Int32).Value = entity.idPadre;
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

        public int EditSubMenu(Menu entity)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaSubMenu", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@Url", MySqlDbType.String, 50).Value = entity.Url;
                        cmd.Parameters.Add("@Orden", MySqlDbType.Int32).Value = entity.Orden;
                        cmd.Parameters.Add("@IdMenu", MySqlDbType.Int32).Value = entity.idMenu;
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

        public List<Menu> GetMenuPerfil(Menu entity)
        {
            List<Menu> list = new List<Menu>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenMenuPerfil", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pidPerfil", MySqlDbType.Int32).Value = entity.idPerfil;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idMenu"] != null)
                        {

                            Menu _menu = new Menu
                            {
                                idMenu = int.Parse(dr["idMenu"].ToString()),
                                idPerfil = int.Parse(dr["idPerfil"].ToString()),
                                isTitle = int.Parse(dr["isTitle"].ToString()),
                                idPadre = int.Parse(dr["idPadre"].ToString()),
                                Icon = (dr["Icon"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Url = (dr["Url"].ToString()),
                                Orden = int.Parse(dr["Orden"].ToString()),
                                Status = int.Parse(dr["Status"].ToString()),
                            };
                            list.Add(_menu);    
                        }
                    }
                }
                return list;
            }
        }

        public List<Menu> GetMenuRol(Menu entity)
        {
            List<Menu> list = new List<Menu>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenMenuRol", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pidRol", MySqlDbType.Int32).Value = entity.idRol;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idMenu"] != null)
                        {

                            Menu _menu = new Menu
                            {
                                idMenu = int.Parse(dr["idMenu"].ToString()),
                                idPerfil = int.Parse(dr["idPerfil"].ToString()),
                                isTitle = int.Parse(dr["isTitle"].ToString()),
                                idPadre = int.Parse(dr["idPadre"].ToString()),
                                Icon = (dr["Icon"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Url = (dr["Url"].ToString()),
                                Orden = int.Parse(dr["Orden"].ToString()),
                                Status = int.Parse(dr["Status"].ToString()),
                            };
                            list.Add(_menu);
                        }
                    }
                }
                return list;
            }
        }

        public List<Menu> GetMenu()
        {
            List<Menu> list = new List<Menu>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenMenu", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idMenu"] != null)
                        {

                            Menu _menu = new Menu
                            {
                                idMenu = int.Parse(dr["idMenu"].ToString()),
                                isTitle = int.Parse(dr["isTitle"].ToString()),
                                idPadre = int.Parse(dr["idPadre"].ToString()),
                                Icon = (dr["Icon"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Url = (dr["Url"].ToString()),
                                Orden = int.Parse(dr["Orden"].ToString()),
                                Status = int.Parse(dr["Status"].ToString()),
                            };
                            list.Add(_menu);
                        }
                    }
                }
                return list;
            }
        }
    }
}
