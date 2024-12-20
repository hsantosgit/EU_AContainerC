using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.OutputCaching;

namespace EU_CottonContainer.Controllers
{
    public class SeguridadController : Controller
    {
        private static Usuario _user;

        [HttpPost]
        [OutputCache]
        public ActionResult Index(string usuario, string bandera)
        {
            _user = UsuarioFacade.GetUsuarioByUserName(usuario);
            List<Token> _listTokens = TokenFacade.ObtenTokenList();
            //Validamos que el usuario no tenga un token registrado
            if (_listTokens.FindAll(x => x.idUsuario == _user.idUsuario).Count > 0 && bandera != "1")
            {
                return RedirectToAction("Salir", "Home");
            }

            Random random = new Random();
            _user.Token = Convert.ToString(random.Next(100000, 999999));

            //Guardamos el token actual para evitar el reenvío de tokens para el mismo usuario
            if (bandera != "1")
            {
                UsuarioFacade.SaveToken(_user);
            }

            _user.Sesion = bandera;
            if (bandera == "0")
            {
                //Petición que viene de Inicio de Sesión Normal

                if (_user.IsTokenSMS >= 3)
                {
                    mailService.SendMail(_user.CorreoAlterno, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo alterno:  " + _user.CorreoAlterno, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
                else if (_user.IsTokenMail == 1)
                {
                    //Ejecutar el proceso de envío de código por Email
                    //this.SendMail();
                    mailService.SendMail(_user.Correo, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo:  " + _user.Correo, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
            }
            else if (bandera == "1")
            {
                // Peticion que viene de la solicitud de recuperar contraseña
                if (_user.IsTokenSMS >= 3)
                {
                    mailService.SendMail(_user.CorreoAlterno, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo alterno:  " + _user.CorreoAlterno, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });

                }
                else if (_user.IsTokenMail == 1)
                {
                    //Ejecutar el proceso de envío de código por Email
                    mailService.SendMail(_user.Correo, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo:  " + _user.Correo, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
            }
            else if (bandera == "2")
            {
                //Petición que viene de Inicio de Sesión pero con Cambio de contraseña
                if (_user.IsTokenSMS >= 3)
                {
                    mailService.SendMail(_user.CorreoAlterno, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo alterno:  " + _user.CorreoAlterno, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
                else if (_user.IsTokenMail == 1)
                {
                    //Ejecutar el proceso de envío de código por Email
                    //this.SendMail();
                    mailService.SendMail(_user.Correo, _user.Token);
                    SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo:  " + _user.Correo, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
            }
            return View(_user);
        }

        protected void SendsmsMessage()
        {
            ////twilio
            //smsService.SendMessage(_user.Telefono, _user.Token);

            //smsMasivo
            smsMasivo.SendMessage(_user.Telefono, _user.Token);
        }

        [HttpPost]
        public string Validate(Usuario user)
        {
            string resp = string.Empty;
            try
            {
                if (user.TokenValidate == _user.Token)
                {
                    if (_user.Sesion == "0")
                    {
                        List<Claim> c = new List<Claim>() { new Claim(ClaimTypes.Name, _user.userName) };

                        ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties p = new();

                        p.AllowRefresh = true;
                        p.IsPersistent = _user.MantenerActivo;
                        p.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);

                        //if (!_user.MantenerActivo)
                        //    p.ExpiresUtc = DateTime.UtcNow.AddMinutes(1);
                        //else
                        //    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);

                        //Actualizar el uso del token para el usuario
                        SeguridadFacade.UpdateToken(_user.idUsuario, 1);
                        //Se restablecen los intenos de IsTokenSMS
                        SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 0);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Ingreso al sistema: " + _user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else if (_user.Sesion == "1")
                    {
                        SeguridadFacade.UpdateExpiredPass(new Usuario { userName = _user.userName, Password = _user.userName });
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Usuario solicitó recuperación de contraseña: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "CA/" + _user.userName;
                    }
                    else if (_user.Sesion == "2")
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Usuario solicitó cambio de contraseña: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "CC/" + _user.userName;
                    }
                }
                else
                {
                    resp = "Código de validación incorrecto.";
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Código de validación incorrecto: " + _user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Modulo de Autenticación \n" + "Usuario autenticado: " + new cryptoSHA256().Decrypt(user.userName + "\n" + "Error: " + ex.Message));
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public ActionResult CambioContrasena(string usuario)
        {
            this.CargarUsuario(usuario);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string UpdatePass(Usuario user)
        {
            string resp = string.Empty;
            try
            {
                Regex regex = new Regex("^(?=.{8,25}$)(?=(?:.*[0-9]){1})(?=(?:.*[.?,;_¡!¿*% &$(){}]){1})(?=.*[A-Z])(?=.*[a-z])(?!.*(.)\\1).+$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(user.Password) || user.IsToken == 1)
                {
                    if (user.Password == user.ConfirmPassword)
                    {
                        user.userName = _user.userName;
                        if (UsuarioFacade.UpdateUser(user) > 0)
                        {
                            SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 0);
                            BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Actualización contraseña: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                            resp = "OK";
                        }
                        else
                        {
                            return "Ocurrio un problema con el cambio de contraseña.";
                        }
                    }
                    else
                    {
                        resp = "Las contraseñas no coinciden. Favor de validar.";
                    }
                }
                else
                {
                    resp = "La contraseña no cumple con las políticas establecidas. Favor de validar.";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Modulo de Cambio Contraseña \n" + "Usuario cambio contraseña: " + _user.userName + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        public ActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        public string RestaurarContrasena(Usuario user)
        {
            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(user.Correo))
                {
                    int result = SeguridadFacade.SolicitudCambio(user);
                    if (result == 0)
                    {
                        resp = "Datos de solicitud incorrectos, favor de validar";
                    }
                    else
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Recuperación de contraseña usuario: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK/" + string.Format("?usuario={0}&bandera={1}", user.userName, "1");
                    }
                }
                else
                {
                    resp = "Debes capturar ya sea tu número de teléfono o tu correo.";
                }

            }
            catch (Exception ex)
            {
                logClass.WriteLog("Modulo de recuperación de contraseña \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        public ActionResult SolicitaCambio()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (!c.Identity.IsAuthenticated)
                    return RedirectToAction("Salir", "Home");
            }

            this.CargarUsuario(c.FindFirst(ClaimTypes.Name).Value);
            Random random = new Random();
            _user.Token = Convert.ToString(random.Next(100000, 999999));

            if (_user.IsTokenSMS >= 3)
            {
                mailService.SendMail(_user.CorreoAlterno, _user.Token);
                SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo alterno:  " + _user.CorreoAlterno, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
            }
            else if (_user.IsTokenMail == 1)
            {
                //Ejecutar el proceso de envío de código por Email
                mailService.SendMail(_user.Correo, _user.Token);
                SeguridadFacade.UpdateTokenSMS(_user.idUsuario, 1);
                BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 3, Accion = "Se envío Token(" + _user.Token + "), usuario : " + _user.userName + ", correo:  " + _user.Correo, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
            }

            return View(_user);
        }

        [HttpPost]
        public string ActualizarContrasenaUsuario(Usuario user)
        {
            string resp = string.Empty;
            try
            {
                if (user.TokenValidate == _user.Token)
                {
                    SeguridadFacade.UpdateExpiredPass(new Usuario { userName = _user.userName, Password = _user.userName });
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Usuario solicitó cambio de contraseña: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "CA/" + user.userName;
                }
                else
                {
                    resp = "Código de validación incorrecto. Favor de validar";
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Código de validación incorrecto: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Modulo de actualización de contraseña \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        protected void CargarUsuario(string username)
        {
            _user = UsuarioFacade.GetUsuarioByUserName(username);
        }


    }
}
