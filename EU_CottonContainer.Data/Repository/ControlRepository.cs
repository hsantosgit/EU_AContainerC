using EU_CottonContainer.Model;
using EU_CottonSecurity;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class ControlRepository
    {
        public List<Rol> GetRolList()
        {
            List<Rol> _lstRol = new List<Rol>();
            Rol _rol = new Rol();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenRolList", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Nombre"] != null)
                        {
                            _rol = new Rol
                            {
                                idRol = int.Parse(dr["idRol"].ToString()),
                                Nombre = dr["Nombre"].ToString()
                            };
                            _lstRol.Add(_rol);
                        }
                    }
                }
            }
            return _lstRol;
        }

        public List<Planta> GetPlantaList()
        {
            List<Planta> _lstPlanta = new List<Planta>();
            Planta _planta = new Planta();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenPlantaList", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Nombre"] != null)
                        {
                            _planta = new Planta
                            {
                                idPlanta = int.Parse(dr["idPlanta"].ToString()),
                                Nombre = dr["Nombre"].ToString()
                            };
                            _lstPlanta.Add(_planta);
                        }
                    }
                }
            }
            return _lstPlanta;
        }

        public List<Aplicacion> GetAplicacionList()
        {
            List<Aplicacion> _lstAplicacion = new List<Aplicacion>();
            Aplicacion _Aplicacion = new Aplicacion();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenAplicacionesList", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Nombre"] != null)
                        {
                            _Aplicacion = new Aplicacion
                            {
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                Url = dr["Url"].ToString(),
                                icono = dr["icono"].ToString(),
                                Status = int.Parse(dr["Status"].ToString()),
                            };
                            _lstAplicacion.Add(_Aplicacion);
                        }
                    }
                }
            }
            return _lstAplicacion;
        }

        public List<Usuario> GetStatusUserList()
        {
            List<Usuario> _lstStatus = new List<Usuario>();
            Usuario _Usuario = new Usuario();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenStatusUsuarioList", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["status"] != null)
                        {
                            _Usuario = new Usuario
                            {
                                Status = int.Parse(dr["idStatus"].ToString()),
                                Abreviatura = dr["status"].ToString()
                            };
                            _lstStatus.Add(_Usuario);
                        }
                    }
                }
            }
            return _lstStatus;
        }

        public List<Aplicacion> GetAplicacionListBy(string _sUser)
        {
            List<Aplicacion> _lstAplicacion = new List<Aplicacion>();
            Aplicacion _Aplicacion = new Aplicacion();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenAplicacionesListBy", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = _sUser;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Nombre"] != null)
                        {
                            _Aplicacion = new Aplicacion
                            {
                                idAplicacion = int.Parse(dr["idAplicacion"].ToString()),
                                Nombre = dr["Nombre"].ToString()
                            };
                            _lstAplicacion.Add(_Aplicacion);
                        }
                    }
                }
            }
            return _lstAplicacion;
        }

        //public List<Aplicacion> GetAplicacionList()
        //{
        //    return AplicacionFactory.GetList((DbDataReader)base._ProviderDB.GetDataReader("sp_ObtenAplicacionesList", new DbParameter[] { }));
        //}

    }
}
