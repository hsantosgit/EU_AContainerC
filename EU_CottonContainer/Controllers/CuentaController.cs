using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Data;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    public class CuentaController : Controller
    {
        private readonly Contexto _contexto;
        private static Usuario _user;
        private static List<Usuario> _lstUsers;
        public CuentaController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public IActionResult Login()
        {
            //ClaimsPrincipal c = HttpContext.User;
            //if (c.Identity != null)
            //{
            //    if (c.Identity.IsAuthenticated)
            //        return RedirectToAction("Index", "Home");
            //}

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache]
        public string Login(Usuario _user)
        {
            try
            {
                if (IsReCaptchValid())
                {
                    this.GetAllUsers();
                    List<Usuario> _lstUser = _lstUsers.FindAll(x => x.userName == _user.userName);
                    if (_lstUser.Count > 0)
                    {
                        //Se registra el intento de ingreso del usuario 
                        SeguridadFacade.UpdateTries(new Usuario { userName = _user.userName, Status = 1 });
                        DateTime FechaActual = DateTime.Now;
                        if (!String.IsNullOrEmpty(_lstUser[0].FechaBLQ))
                        {
                            DateTime _fechaBLQ = DateTime.Parse(_lstUser[0].FechaBLQ);
                            if (FechaActual >= _fechaBLQ && _lstUser[0].Status == Convert.ToInt32(variables.EnumStatusUser.Bloqueado))
                            {
                                //Se desbloquea el usuario por caducidad de tiempo
                                UsuarioFacade.ActualizaStatus(new Usuario { userName = _user.userName, Status = Convert.ToInt32(variables.EnumStatusUser.Activo) });
                                //Se actualiza el número de intentos
                                SeguridadFacade.UpdateTries(new Usuario { userName = _user.userName, Status = 0 });
                                //Se guarda en bítacora el desbloqueo del usuario
                                BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _lstUser[0].idUsuario, idMenu = 3, Accion = "Desbloqueo automático por tiempo, usuario: " + _user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                this.GetAllUsers();
                            }
                        }
                    }
                    //Si el usuario sobrepasa los 5 intentos de ingreso, se bloquea su usuario
                    if (_lstUsers.FindAll(x => x.userName == _user.userName && x.Intentos >= 5).Count > 0)
                    {
                        _lstUser = _lstUsers.FindAll(x => x.userName == _user.userName && x.Intentos >= 5);
                        if (_lstUser[0].Status == 5)
                        {
                            BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _lstUser[0].idUsuario, idMenu = 3, Accion = "Intento Iniciar Sesión: " + _user.userName + ", pero esta bloqueado.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                            return "El usuario: " + _user.userName + " a sido bloqueado por número de intentos fallidos. Favor de comunicarse con el administrador para su desbloqueo.";
                        }
                        else
                        {
                            //Se actualiza el status del usuario a bloqueado
                            UsuarioFacade.ActualizaStatus(new Usuario { userName = _user.userName, Status = Convert.ToInt32(variables.EnumStatusUser.Bloqueado) });
                            //Se guarda en bítacora el bloqueo del usuario
                            BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _lstUser[0].idUsuario, idMenu = 3, Accion = "Intento Iniciar Sesión: " + _user.userName + ", pero esta bloqueado.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                            return "El usuario: " + _user.userName + " a sido bloqueado por número de intentos fallidos. Favor de comunicarse con el administrador para su desbloqueo.";
                        }
                    }
                    // si al usuario ya se le envíaron 3 notificaciones a su mail principal y dos mas a su mail alterno
                    else if (_lstUsers.FindAll(x => x.userName == _user.userName && x.IsTokenSMS >= 5).Count > 0)
                    {
                        _lstUser = _lstUsers.FindAll(x => x.userName == _user.userName && x.IsTokenSMS >= 5);
                        //Se actualiza el status del usuario a bloqueado
                        UsuarioFacade.ActualizaStatus(new Usuario { userName = _user.userName, Status = Convert.ToInt32(variables.EnumStatusUser.Bloqueado) });
                        //Se guarda en bítacora el bloqueo del usuario
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _lstUser[0].idUsuario, idMenu = 3, Accion = "Se bloqueo el usuario: " + _user.userName + ", por número de intentos fallidos en la captura del código de acceso.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        return "El usuario: " + _user.userName + " a sido bloqueado por sobrepasar el envío de códigos de autenticación. Favor de comunicarse con al administrador para el desbloqueo.";
                    }
                    else
                    {
                        Usuario _xuser = UsuarioFacade.AutentificarUsuario(_user);
                        string url = string.Empty;
                        if (_xuser.Nombre != null)
                        {
                            SeguridadFacade.UpdateTries(new Usuario { userName = _xuser.userName, Status = 0 });

                            if (IsPasswordActive(_xuser.FechaPass))
                            {
                                if (!String.IsNullOrEmpty(_xuser.FechaToken))
                                {
                                    DateTime FechaActual = DateTime.Now;
                                    DateTime _fechaTOK = DateTime.Parse(_xuser.FechaToken);
                                    //Validar si el Token sigue activo(Por día).
                                    if (FechaActual > _fechaTOK)
                                    {
                                        //Se actualiza token por caducidad de tiempo
                                        SeguridadFacade.UpdateToken(_xuser.idUsuario, 0);
                                        _xuser.IsTokenActive = 0;
                                    }
                                }

                                switch (_xuser.Status)
                                {
                                    case 1:  //Usuario Activo                                            
                                        if (_xuser.IsToken == 0 || _xuser.IsTokenActive == 1)
                                        {
                                            
                                            List<Claim> c = new List<Claim>() { new Claim(ClaimTypes.Name, _user.userName) };

                                            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                            AuthenticationProperties p = new();

                                            p.AllowRefresh = true;
                                            p.IsPersistent = _user.MantenerActivo;
                                            p.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);

                                            //if (!_user.MantenerActivo)
                                            //    p.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
                                            //else
                                            //    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(20);

                                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                            SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 0);

                                            BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "Ingreso sistema: " + _xuser.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });

                                            url = "OK";
                                        }
                                        else
                                        {
                                            //Borramos sus Tokens si tiene alguno habilitado
                                            TokenFacade.DelUserToken(_user.idUsuario);
                                            //Ahora debe pasar a la autenticación por mail 
                                            url = "AU/" + string.Format("?usuario={0}&bandera={1}", _xuser.userName, "0");
                                        }
                                        break;
                                    case 2: //Baja
                                        url = "Credenciales de acceso incorrectas.";
                                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "intento ingresar sistema: " + _xuser.userName + ", pero ha sido dado de baja.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                        break;
                                    case 3: //Sin Uso 
                                        url = "CC/" + string.Format("{0}?pundbcott={1}", "CambioContrasena", new cryptoSHA256().Encrypt(_xuser.userName));
                                        break;
                                    case 4: //Contraseña
                                        //Borramos sus Tokens si tiene alguno habilitado
                                        TokenFacade.DelUserToken(_xuser.idUsuario);
                                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "Cambio de contraseña: " + _xuser.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                        url = "CO/" + string.Format("?usuario={0}&bandera={1}", _xuser.userName, "2");
                                        break;
                                    case 5: //Bloqueado
                                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "intento ingresar sistema: " + _xuser.userName + ", pero ha sido bloqueado.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                        url = "El usuario: " + _user.userName + " a sido bloqueado por número de intentos fallidos. Favor de comunicarse con el administrador para su desbloqueo.";
                                        break;
                                }
                                return url;
                            }
                            else
                            {
                                //Actualizar la contraseña, para solicitar el cambio en el proximo reinicio
                                SeguridadFacade.UpdateExpiredPass(new Usuario { userName = _xuser.userName, Password = _xuser.userName });
                                BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "Se restablecio la contraseña del usuario: " + _xuser.userName + ", por vencimiento de vigencia.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                return "Contraseña expirada.\n Se a restablecido su contraseña. \n Contraseña temporal: " + _xuser.userName + "\n Se le pedirá la actualice en su próximo ingreso.";
                            }
                        }
                        else
                        {
                            BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _xuser.idUsuario, idMenu = 3, Accion = "Intento Iniciar Sesión: " + _user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                            return "Credenciales de acceso incorrectas.";
                        }
                    }
                }
                else
                {
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "Intento Iniciar Sesión: " + _user.userName + ", captcha incorrecto.", ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    return "Captcha incorrecto.";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Modulo de Ingreso al Sistema \n" + "Usuario intento ingresar: " + _user.userName + "\n" + "Error: " + ex.Message);
                return GlobalConfiguration.ExceptionError;
            }
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = variables.key;
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        public bool IsPasswordActive(string Fecha)
        {
            DateTime FechaPass = Convert.ToDateTime(Fecha);
            FechaPass = FechaPass.AddDays(variables.PassActiveDays);
            DateTime FechaActual = DateTime.Now;
            if (FechaActual <= FechaPass)
                return true;
            else
                return false;
        }

        public void GetAllUsers()
        {
            _lstUsers = UsuarioFacade.GetAllUsers();
        }

    }
}
