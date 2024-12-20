using EU_CottonSecurity;
using EU_CottonContainer.Model;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class BitacoraRepository
    {
        public static void Add(Bitacora entity)
        {
            try
            {
                Usuario _User = new Usuario();

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_AltaBitacora", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuario", MySqlDbType.Int32).Value = entity.idUsuario;
                        cmd.Parameters.Add("@idMenu", MySqlDbType.Int32).Value = entity.idMenu;
                        cmd.Parameters.Add("@ipAddress", MySqlDbType.String, 20).Value = entity.ipAddress;
                        cmd.Parameters.Add("@mcAddress", MySqlDbType.String, 45).Value = entity.mcAddress;
                        cmd.Parameters.Add("@Ubicacion", MySqlDbType.String, 45).Value = entity.Ubicacion;
                        cmd.Parameters.Add("@Accion", MySqlDbType.String, 350).Value = entity.Accion;
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
    }
}
