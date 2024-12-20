using EU_CottonContainer.Model;
using MySql.Data.MySqlClient;
using EU_CottonSecurity;

namespace EU_CottonContainer.Data.Repository
{
    public class UsuarioRepository
    {
        public int Add(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaUsuario", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@userName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@apPaterno", MySqlDbType.String, 50).Value = entity.apPaterno;
                        cmd.Parameters.Add("@apMaterno", MySqlDbType.String, 50).Value = entity.apMaterno;
                        cmd.Parameters.Add("@Telefono", MySqlDbType.String, 20).Value = entity.Telefono;
                        cmd.Parameters.Add("@Correo", MySqlDbType.String, 100).Value = entity.Correo;
                        cmd.Parameters.Add("@CorreoAlterno", MySqlDbType.String, 100).Value = entity.CorreoAlterno;
                        cmd.Parameters.Add("@Sesion", MySqlDbType.String, 5).Value = entity.Sesion;
                        cmd.Parameters.Add("@Status", MySqlDbType.Int32).Value = entity.Status;
                        cmd.Parameters.Add("@Pass", MySqlDbType.String, 500).Value = new cryptoSHA256().Encrypt(entity.userName);
                        cmd.Parameters.Add("@idRol", MySqlDbType.Int32).Value = entity.idRol;
                        cmd.Parameters.Add("@idPlanta", MySqlDbType.Int32).Value = entity.idPlanta;
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

        public int Edit(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaUsuario", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@userName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@Nombre", MySqlDbType.String, 50).Value = entity.Nombre;
                        cmd.Parameters.Add("@apPaterno", MySqlDbType.String, 50).Value = entity.apPaterno;
                        cmd.Parameters.Add("@apMaterno", MySqlDbType.String, 50).Value = entity.apMaterno;
                        cmd.Parameters.Add("@Telefono", MySqlDbType.String, 20).Value = entity.Telefono;
                        cmd.Parameters.Add("@Correo", MySqlDbType.String, 100).Value = entity.Correo;
                        cmd.Parameters.Add("@CorreoAlterno", MySqlDbType.String, 100).Value = entity.Correo;
                        cmd.Parameters.Add("@Status", MySqlDbType.Int32).Value = entity.Status;
                        cmd.Parameters.Add("@idRol", MySqlDbType.Int32).Value = entity.idRol;
                        cmd.Parameters.Add("@idPlanta", MySqlDbType.Int32).Value = entity.idPlanta;
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

        public int AddConfigUser(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaUsuarioConfig", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pPass", MySqlDbType.String, 500).Value = new cryptoSHA256().Encrypt(entity.userName);
                        cmd.Parameters.Add("@pIsToken", MySqlDbType.Int32).Value = entity.IsToken;
                        cmd.Parameters.Add("@pIsTokenSMS", MySqlDbType.Int32).Value = entity.IsTokenSMS;
                        cmd.Parameters.Add("@pIsTokenMail", MySqlDbType.Int32).Value = entity.IsTokenMail;
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

        public int EditConfigUser(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaUsuarioConfig", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pIsToken", MySqlDbType.Int32).Value = Convert.ToInt32(entity.boolToken);
                        cmd.Parameters.Add("@pIsTokenSMS", MySqlDbType.Int32).Value = Convert.ToInt32(entity.boolTokenSMS);
                        cmd.Parameters.Add("@pIsTokenMail", MySqlDbType.Int32).Value = Convert.ToInt32(entity.boolTokenMail);
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

        public int AddUserApps(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaUsuarioAplicacion", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pUsuario", MySqlDbType.String, 45).Value = entity.Nombre;
                        cmd.Parameters.Add("@pidAplicacion", MySqlDbType.Int32).Value = entity.IsToken;
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

        public int EditUserApps(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                //Elimina la configuración e inserta la nueva
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaUsuarioAplicacion", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuario", MySqlDbType.Int32).Value = entity.idUsuario;
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

        public List<Usuario> GetAll()
        {
            Usuario _User = new Usuario();
            List<Usuario> _lstUsers = new List<Usuario>();
            try
            {
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ObtenUsuarios", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr["userName"] != null)
                            {
                                _User = new Usuario
                                {
                                    idUsuario = int.Parse(dr["idUsuario"].ToString()),
                                    userName = dr["userName"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    apPaterno = (dr["apPaterno"].ToString()),
                                    apMaterno = (dr["apMaterno"].ToString()),
                                    Telefono = (dr["Telefono"].ToString()),
                                    Correo = (dr["Correo"].ToString()),
                                    CorreoAlterno = (dr["CorreoAlterno"].ToString()),
                                    Sesion = (dr["Sesion"].ToString()),
                                    Status = int.Parse(dr["Status"].ToString()),
                                    Abreviatura = dr["Abreviatura"].ToString(),
                                    FechaAlta = (dr["FechaAlta"].ToString()),
                                    idRol = int.Parse(dr["idRol"].ToString()),
                                    Rol = dr["Rol"].ToString(),
                                    idPlanta = int.Parse(dr["idPlanta"].ToString()),
                                    Planta = dr["Planta"].ToString(),
                                    FechaPass = dr["FechaPass"].ToString(),
                                    FechaBLQ = dr["FechaBLQ"].ToString(),
                                    IsToken = int.Parse(dr["IsToken"].ToString()),
                                    IsTokenSMS = int.Parse(dr["IsTokenSMS"].ToString()),
                                    IsTokenMail = int.Parse(dr["IsTokenMail"].ToString()),
                                    boolToken = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                    boolTokenSMS = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                    boolTokenMail = Convert.ToBoolean(int.Parse(dr["IsTokenMail"].ToString())),
                                    Intentos = int.Parse(dr["Intentos"].ToString()),
                                    IsTokenActive = int.Parse(dr["IsTokenActive"].ToString()),
                                    FechaToken = (dr["FechaToken"].ToString()),
                                };
                                _lstUsers.Add(_User);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _lstUsers[0].Token = ex.Message;    
                logClass.WriteLog("Repositorio Usuario / GetAll() \n" + "Error: " + ex.Message);
            }

            return _lstUsers;

        }

        public Usuario GetBy(Usuario entity)
        {
            Usuario _User = new Usuario();


            //test encryptor = new test();
            //string encryptAES = encryptor.Encrypt(entity.Password);

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_AutenticarUsuario", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", MySqlDbType.String, 45).Value = entity.userName;
                    cmd.Parameters.Add("@contrasena", MySqlDbType.String, 500).Value = new cryptoSHA256().Encrypt(entity.Password);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["userName"] != null && entity.userName != null)
                        {
                            _User = new Usuario
                            {
                                idUsuario = int.Parse(dr["idUsuario"].ToString()),
                                userName = dr["userName"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                apPaterno = (dr["apPaterno"].ToString()),
                                apMaterno = (dr["apMaterno"].ToString()),
                                Telefono = (dr["Telefono"].ToString()),
                                Correo = (dr["Correo"].ToString()),
                                CorreoAlterno = (dr["CorreoAlterno"].ToString()),
                                Sesion = (dr["Sesion"].ToString()),
                                Status = int.Parse(dr["Status"].ToString()),
                                Abreviatura = dr["Abreviatura"].ToString(),
                                FechaAlta = (dr["FechaAlta"].ToString()),
                                idRol = int.Parse(dr["idRol"].ToString()),
                                Rol = dr["Rol"].ToString(),
                                idPlanta = int.Parse(dr["idPlanta"].ToString()),
                                Planta = dr["Planta"].ToString(),
                                FechaPass = dr["FechaPass"].ToString(),
                                FechaBLQ = dr["FechaBLQ"].ToString(),
                                IsToken = int.Parse(dr["IsToken"].ToString()),
                                IsTokenSMS = int.Parse(dr["IsTokenSMS"].ToString()),
                                IsTokenMail = int.Parse(dr["IsTokenMail"].ToString()),
                                boolToken = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                boolTokenSMS = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                boolTokenMail = Convert.ToBoolean(int.Parse(dr["IsTokenMail"].ToString())),
                                Intentos = int.Parse(dr["Intentos"].ToString()),
                                IsTokenActive = int.Parse(dr["IsTokenActive"].ToString()),
                                FechaToken = (dr["FechaToken"].ToString()),
                            };
                        }
                    }
                }

                return _User;
            }
        }

        public Usuario GetUser(Usuario entity)
        {
            Usuario _User = new Usuario();


            return _User;
        }


        public Usuario GetByName(string name)
        {
            Usuario _User = new Usuario();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenUsuarioUserName", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", MySqlDbType.String, 45).Value = name;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["userName"] != null)
                        {
                            _User = new Usuario
                            {
                                idUsuario = int.Parse(dr["idUsuario"].ToString()),
                                userName = dr["UserName"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                apPaterno = (dr["apPaterno"].ToString()),
                                apMaterno = (dr["apMaterno"].ToString()),
                                Telefono = (dr["Telefono"].ToString()),
                                Correo = (dr["Correo"].ToString()),
                                CorreoAlterno = (dr["CorreoAlterno"].ToString()),
                                Sesion = (dr["Sesion"].ToString()),
                                Status = int.Parse(dr["Status"].ToString()),
                                Abreviatura = dr["Abreviatura"].ToString(),
                                FechaAlta = (dr["FechaAlta"].ToString()),
                                idRol = int.Parse(dr["idRol"].ToString()),
                                Rol = dr["Rol"].ToString(),
                                idPlanta = int.Parse(dr["idPlanta"].ToString()),
                                Planta = dr["Planta"].ToString(),
                                FechaPass = dr["FechaPass"].ToString(),
                                FechaBLQ = dr["FechaBLQ"].ToString(),
                                IsToken = int.Parse(dr["IsToken"].ToString()),
                                IsTokenSMS = int.Parse(dr["IsTokenSMS"].ToString()),
                                IsTokenMail = int.Parse(dr["IsTokenMail"].ToString()),
                                boolToken = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                boolTokenSMS = Convert.ToBoolean(int.Parse(dr["IsTokenSMS"].ToString())),
                                boolTokenMail = Convert.ToBoolean(int.Parse(dr["IsTokenMail"].ToString())),
                                Intentos = int.Parse(dr["Intentos"].ToString()),
                                IsTokenActive = int.Parse(dr["IsTokenActive"].ToString()),
                                FechaToken = (dr["FechaToken"].ToString()),
                            };
                        }
                    }
                }

                return _User;
            }
        }

        public int Remove(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaContrasena", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pPass", MySqlDbType.String, 500).Value = new cryptoSHA256().Encrypt(entity.Password);
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

        //public int UpdateSesion(Usuario entity)
        //{
        //    try
        //    {
        //        return Convert.ToInt32(base._ProviderDB.ExecuteNonQuery("sp_ActualizaSesion",
        //            new DbParameter[]
        //            {
        //                DataFactory.GetObjParameter(GlobalConfiguration.ProviderDB, "@pUserName", DbType.String, entity.userName, 45),
        //                DataFactory.GetObjParameter(GlobalConfiguration.ProviderDB, "@pSesion", DbType.String, entity.Sesion, 5)
        //            }));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public int UpdateStatus(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaStatus", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pStatus", MySqlDbType.Int32).Value = entity.Status;
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

        public int SaveToken(Usuario entity)
        {
            try
            {
                Usuario _User = new Usuario();
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaToken", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pidUsuario", MySqlDbType.Int32).Value = entity.idUsuario;
                        cmd.Parameters.Add("@pToken", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Token);

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
