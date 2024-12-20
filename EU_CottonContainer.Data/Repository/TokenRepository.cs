using EU_CottonContainer.Model;
using EU_CottonSecurity;
using MySql.Data.MySqlClient;

namespace EU_CottonContainer.Data.Repository
{
    public class TokenRepository
    {
        public List<Token> GetTokenList()
        {
            List<Token> list = new List<Token>();

            using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_ObtenToken", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["idUsuario"] != null)
                        {
                            Token _rol = new Token
                            {
                                idUsuario = int.Parse(dr["idUsuario"].ToString()),
                            };
                            list.Add(_rol);
                        }
                    }
                }
                return list;
            }
        }

        public int DelUserToken(int idUsuario)
        {
            try
            {
                int dr;

                using (MySqlConnection con = new(GlobalConfiguration.StringConectionDB))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_ActualizaUserToken", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdUsuario", MySqlDbType.Int32).Value = idUsuario;
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
