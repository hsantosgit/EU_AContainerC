using EU_CottonSecurity;
using EU_CottonContainer.Model;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace EU_CottonContainer.Data.Repository
{
    public class SeguridadRepository
    {
        public int SolicitudCambio(Usuario entity)
        {
            int resp = 0;
            try
            {
                
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_SolicitudCambioContrasena", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pCorreo", MySqlDbType.String, 45).Value = entity.Correo;
                        cmd.Parameters.Add("@pPass", MySqlDbType.String, 500).Value = new cryptoSHA256().Encrypt(entity.userName);

                        con.Open();
                        resp = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

            }
            catch
            {
                throw;
            }
            return resp;
        }

        public static void UpdateExpiredPass(Usuario entity)
        {
            try
            {
                using(MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActaulizaContrasenaExpirada", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pPassword", MySqlDbType.String,500).Value = new cryptoSHA256().Encrypt(entity.userName);

                        con.Open();
                        var dr = cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateTries(Usuario entity)
        {
            try
            {
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaIntentos", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUserName", MySqlDbType.String, 45).Value = entity.userName;
                        cmd.Parameters.Add("@pBandera", MySqlDbType.Int32).Value = entity.Status;

                        con.Open();
                        var dr = cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateToken(int idUsuario, int Bandera)
        {
            try
            {
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaToken", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuario", MySqlDbType.Int32).Value = idUsuario;
                        cmd.Parameters.Add("@Bandera", MySqlDbType.Int32).Value = Bandera;

                        con.Open();
                        var dr = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateTokenSMS(int idUsuario, int Bandera)
        {
            try
            {
                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaTokenSMS", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuario", MySqlDbType.Int32).Value = idUsuario;
                        cmd.Parameters.Add("@Bandera", MySqlDbType.Int32).Value = Bandera;

                        con.Open();
                        var dr = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
