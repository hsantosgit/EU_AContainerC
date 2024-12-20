using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EU_CottonContainer.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private static Usuario _user;
        private static Usuario _userEdit;
        private static List<Usuario> _lstUsers = new List<Usuario>();
        List<Rol> _lstRol = new List<Rol>();
        List<Planta> _lstPlanta = new List<Planta>();
        List<Aplicacion> _lstApp = new List<Aplicacion>();
        List<Usuario> _lstStatus = new List<Usuario>();

        public List<string> AppIds { get; set; }
        // GET: UsuarioController
        public ActionResult Index()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);

            //Validar que el usuario tenga accesso a la página
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }

            _lstUsers = UsuarioFacade.GetAllUsers();
            return View(_lstUsers);
        }



        public ActionResult AddUser()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);

            //Validar que el usuario tenga accesso a la página
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }

            _lstRol = ControlFacade.GetAllRoles();
            _lstRol.Insert(0, new Rol { idRol = 0, Nombre = "Seleccione Rol" });
            ViewBag.stores = _lstRol;

            _lstPlanta = ControlFacade.GetAllPlantas();
            _lstPlanta.Insert(0, new Planta { idPlanta = 0, Nombre = "Seleccione Planta" });
            ViewBag.planta = _lstPlanta;

            _lstApp = ControlFacade.GetAllAplicaciones().FindAll(x => x.Status == 1);
            ViewBag.Aplicacion = _lstApp;

            _lstStatus = ControlFacade.GetStatusUsuario();
            ViewBag.Status = _lstStatus;

            return View();
        }

        //[HttpGet]
        //public ActionResult AEditUser(string _sUser)
        //{

        //}

        [HttpPost]
        public ActionResult EditUser(string _sUser)
        {
            _lstRol = ControlFacade.GetAllRoles();
            _lstRol.Insert(0, new Rol { idRol = 0, Nombre = "Seleccione Rol" });
            ViewBag.stores = _lstRol;

            _lstPlanta = ControlFacade.GetAllPlantas();
            _lstPlanta.Insert(0, new Planta { idPlanta = 0, Nombre = "Seleccione Planta" });
            ViewBag.planta = _lstPlanta;

            List<Aplicacion> _lstAppUser = ControlFacade.GetAllAplicacionesBy(_sUser);
            _lstApp = ControlFacade.GetAllAplicaciones().FindAll(x => x.Status == 1);
            List<Aplicacion> _lstAppResult = new List<Aplicacion>();
            Aplicacion _auxApp = new Aplicacion();
            foreach (var app in _lstApp)
            {
                foreach (var item in _lstAppUser)
                {
                    if (app.idAplicacion == item.idAplicacion)
                    {
                        app.isChecked = true;
                    }
                }
                _auxApp = new Aplicacion
                {
                    idAplicacion = app.idAplicacion,
                    Nombre = app.Nombre,
                    isChecked = app.isChecked,
                };
                _lstAppResult.Add(_auxApp);
            }
            ViewBag.Aplicacion = _lstAppResult;

            _lstStatus = ControlFacade.GetStatusUsuario();
            ViewBag.Status = _lstStatus;

            this.CargarUsuarioEdit(_sUser);
            return View(_userEdit);
        }

        [HttpPost]
        public string SaveUser(Usuario user)
        {
            string resp = string.Empty;

            try
            {
                if (_lstUsers.FindAll(x => x.Nombre == user.Nombre && x.apPaterno == user.apPaterno && x.apMaterno == user.apMaterno).Count > 0)
                {
                    user.boolName = true;
                }

                if (validaCamposBase(user))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    user.Sesion = "INA";
                    user.Status = 4;
                    user.IsToken = 1;
                    user.IsTokenSMS = 0;
                    user.IsTokenMail = 1;
                    int cont = 0;
                    if (UsuarioFacade.AddUser(user) > 0)
                    {
                        UsuarioFacade.AddUserConfig(user);

                        if (user.AppIds.Count > 0)
                        {
                            foreach (var app in user.AppIds)
                            {
                                cont = Convert.ToInt32(app);
                                UsuarioFacade.AddUserApps(new Usuario { userName = user.userName, Nombre = _user.userName, IsToken = cont });
                            }
                        }
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 4, Accion = "Nuevo registro, usuario: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else
                        resp = GlobalConfiguration.ExceptionError;

                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de usuario \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }

        [HttpPost]
        public string SaveEditUser(Usuario user)
        {
            string resp = string.Empty;
            user.userName = _userEdit.userName;
            user.idUsuario = _userEdit.idUsuario;
            int cont = 0;
            try
            {
                if (_lstUsers.FindAll(x => x.Nombre == user.Nombre && x.apPaterno == user.apPaterno && x.apMaterno == user.apMaterno && x.idUsuario != user.idUsuario).Count > 0)
                {
                    user.boolName = true;
                }

                if (validaCamposBase(user))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    if (UsuarioFacade.EditUser(user) > 0)
                    {
                        //UsuarioFacade.EditUserConfig(user);
                        if (user.AppIds.Count > 0)
                        {
                            UsuarioFacade.EditUserApps(user);
                            foreach (var app in user.AppIds)
                            {
                                cont = Convert.ToInt32(app);
                                UsuarioFacade.AddUserApps(new Usuario { userName = user.userName, Nombre = _user.userName, IsToken = cont });
                            }
                        }
                        resp = "OK";
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = user.idUsuario, idMenu = 3, Accion = "Actualización de Usuario: " + user.userName, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    }
                    else
                        resp = GlobalConfiguration.ExceptionError;
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en actualización de usuario \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateName(string Nombre, string Paterno, string Materno)
        {
            string resp = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(Nombre) && !String.IsNullOrEmpty(Paterno) && !String.IsNullOrEmpty(Materno))
                {
                    if (_lstUsers.FindAll(x => x.Nombre == Nombre && x.apPaterno == Paterno && x.apMaterno == Materno).Count > 0)
                    {
                        resp = "El nombre ya fue ingresado, capture un nombre distinto.";
                        _user.boolName = true;
                    }
                    else
                    {
                        resp = "OK";
                        _user.boolName = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación de nombre de usuario \n" + Nombre + Paterno + Materno + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateUser(string userName)
        {
            string resp = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(userName))
                {
                    userName = userName.ToLower();
                    if (_lstUsers.FindAll(x => x.userName == userName).Count > 0)
                    {
                        resp = "El Usuario ya existe, capture un usuario distinto.";
                        _user.boolUserName = true;
                    }
                    else
                    {
                        resp = "OK";
                        _user.boolUserName = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación de usuario \n" + userName + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateEmail(string Email)
        {
            var regexMail = "[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}";

            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    var match = Regex.Match(Email, regexMail, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        resp = "Capture un correo válido. \n Ejemplo:(user@company.com)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Email))
                        {
                            Email = Email.ToLower();
                            if (_lstUsers.FindAll(x => x.Correo.ToLower() == Email).Count > 0)
                            {
                                resp = "El correo ya se encuentra registrado. \n Capture un correo distinto.";
                                _user.boolEmail = true;
                            }
                            else
                            {
                                resp = "OK";
                                Random random = new Random();
                                _user.Token = Convert.ToString(random.Next(100000, 999999));
                                //mailService.SendMail(Email, _user.Token);
                                mailService.SendMail("hunterhkg16@gmail.com", _user.Token);
                                BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "Se envío Token(" + _user.Token + ") alta de usuario, correo:  " + Email, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                                _user.boolEmail = false;
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el correo para continuar";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del Email \n" + Email + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateEmailAlt(string Email)
        {
            var regexMail = "[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}";
            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    var match = Regex.Match(Email, regexMail, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        resp = "Capture correo válido. ejemplo:(user@company.com)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Email))
                        {
                            if (_lstUsers.FindAll(x => x.CorreoAlterno == Email).Count > 0)
                            {
                                resp = "El correo ya se encuentra registrado. \n Capture un correo distinto.";
                                _user.boolEmailAlterno = true;
                            }
                            else
                            {
                                resp = "OK";
                                _user.boolEmailAlterno = false;
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el correo alterno para continuar";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del email alterno \n" + Email + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidatePhone(string Phone)
        {
            var regexPhone = @"\A[0-9]{10,10}\z";

            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Phone))
                {
                    var match = Regex.Match(Phone, regexPhone, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        resp = "Capture teléfono válido a 10 dígitos. \n ejemplo:(8005551212)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Phone))
                        {
                            Phone = Phone.ToLower();
                            if (_lstUsers.FindAll(x => x.Telefono == Phone).Count > 0)
                            {
                                resp = "El número de teléfono ya se encuentra registrado. \n Capture un número distinto.";
                                _user.boolPhone = true;
                            }
                            else
                            {
                                _user.boolPhone = false;
                                resp = "OK";
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el teléfono para continuar";
                }

            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del Telefono \n" + Phone + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateEmailEdit(string Email, int idUsuario)
        {
            var regexMail = "[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}";
            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    var match = Regex.Match(Email, regexMail, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        resp = "Capture correo válido. ejemplo:(user@company.com)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Email))
                        {
                            if (_lstUsers.FindAll(x => x.Correo == Email && x.idUsuario != idUsuario).Count > 0)
                            {
                                resp = "El correo ya se encuentra registrado en sistema. \n Capture un correo distinto.";
                                _userEdit.boolEmail = true;
                            }
                            else if (_lstUsers.FindAll(x => x.Correo == Email && x.idUsuario == idUsuario).Count > 0)
                            {
                                resp = "correo actual";
                                _userEdit.boolEmail = false;
                            }
                            else
                            {
                                resp = "OK";
                                _userEdit.boolEmail = false;
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el correo para continuar";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del Email \n" + Email + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidateEmailAltEdit(string Email, int idUsuario)
        {
            var regexMail = "[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}";
            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    var match = Regex.Match(Email, regexMail, RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        resp = "Capture correo válido. ejemplo:(user@company.com)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Email))
                        {
                            if (_lstUsers.FindAll(x => x.CorreoAlterno == Email && x.idUsuario != idUsuario).Count > 0)
                            {
                                resp = "El correo ya se encuentra registrado en sistema. \n Capture un correo distinto.";
                                _userEdit.boolEmailAlterno = true;
                            }
                            else if (_lstUsers.FindAll(x => x.CorreoAlterno == Email && x.idUsuario == idUsuario).Count > 0)
                            {
                                resp = "correo actual";
                                _userEdit.boolEmailAlterno = false;
                            }
                            else
                            {
                                resp = "OK";
                                _userEdit.boolEmailAlterno = false;
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el correo alterno para continuar";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del email alterno \n" + Email + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string ValidatePhoneEdit(string Phone, int idUsuario)
        {
            var regexPhone = @"\A[0-9]{10,10}\z";
            var match = Regex.Match(Phone, regexPhone, RegexOptions.IgnoreCase);
            string resp = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Phone))
                {
                    if (!match.Success)
                    {
                        resp = "Capture teléfono válido a 10 dígitos. \n Ejemplo:(8005551212)";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Phone))
                        {
                            if (_lstUsers.FindAll(x => x.Telefono == Phone && x.idUsuario != idUsuario).Count > 0)
                            {
                                resp = "El número de teléfono ya se encuentra registrado. \n Capture un número distinto.";
                                _userEdit.boolPhone = true;
                            }
                            else
                            {
                                _userEdit.boolPhone = false;
                                resp = "OK";
                            }
                        }
                    }
                }
                else
                {
                    resp = "Es necesario capturar el teléfono para continuar";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del Telefono \n" + Phone + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        //protected void CargarUsuario(string username)
        //{

        //}

        protected void CargarUsuarioEdit(string username)
        {
            _userEdit = UsuarioFacade.GetUsuarioByUserName(username);
        }

        protected void ObtenUsuarios()
        {
            _lstUsers = UsuarioFacade.GetAllUsers();
        }

        protected bool validaCamposBase(Usuario _usuario)
        {
            bool valida = false;
            if (_usuario.boolName)
            {
                _user.Nombre = "El nombre que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolUserName)
            {
                _user.Nombre = "El usuario que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolEmail)
            {
                _user.Nombre = "El correo que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolEmailAlterno)
            {
                _user.Nombre = "El correo alterno que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolPhone)
            {
                _user.Nombre = "El teléfono que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.idPlanta == 0)
            {
                _user.Nombre = "Debe Seleccionar a que planta pertenece el usuario.";
                valida = true;
            }
            else if (_usuario.idRol == 0)
            {
                _user.Nombre = "Debe Seleccionar Rol de usuario.";
                valida = true;
            }
            else if (_usuario.AppIds == null)
            {
                _user.Nombre = "Debe seleccionar al menos una aplicación para el usuario.";
                valida = true;
            }

            return valida;
        }

        protected bool validaCamposBaseEdit(Usuario _usuario)
        {
            bool valida = false;
            if (_usuario.boolUserName)
            {
                _userEdit.Nombre = "El usuario que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolEmail)
            {
                _userEdit.Nombre = "El correo que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.boolPhone)
            {
                _userEdit.Nombre = "El teléfono que intenta dar de alta ya se encuentra registrado.";
                valida = true;
            }
            else if (_usuario.idPlanta == 0)
            {
                _userEdit.Nombre = "Debe Seleccionar a que planta pertenece el usuario.";
                valida = true;
            }
            else if (_usuario.idRol == 0)
            {
                _userEdit.Nombre = "Debe Seleccionar Rol de usuario.";
                valida = true;
            }
            else if (_usuario.AppIds == null)
            {
                _userEdit.Nombre = "Debe seleccionar al menos una aplicación para el usuario.";
                valida = true;
            }
            else if (_usuario.boolTokenMail == false && _usuario.boolTokenSMS == false)
            {
                _userEdit.Nombre = "Debe seleccionar al menos un tipo de token para el usuario.";
                valida = true;
            }

            return valida;
        }

        [HttpPost]
        public int setUserStatus(int _status, string _sUser)
        {
            try
            {
                if (_status == 0)
                {
                    //Bloquear el usuario
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "Bloqueo del usuario: " + _sUser, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    return UsuarioFacade.ActualizaStatus(new Usuario { userName = _sUser, Status = (int)variables.EnumStatusUser.Bloqueado });
                }
                else
                {
                    //Desbloquear usuario
                    SeguridadFacade.UpdateTries(new Usuario { userName = _sUser, Status = 0 });
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "Desbloqueo del usuario: " + _sUser, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    return UsuarioFacade.ActualizaStatus(new Usuario { userName = _sUser, Status = (int)variables.EnumStatusUser.Activo });
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Validación de Usuario Key en Base \n" + "Usuario intento ingresar: " + _sUser + "\n" + "Error: " + ex.Message);
                return 0;
            }
        }

        private bool ValidaAccesoUsuario()
        {
            bool valida = false;
            Menu _menu = new Menu { idRol = _user.idRol };
            List<Menu> _lstMenu = MenuFacade.ObtenerMenuRol(_menu);
            int busca = 0;
            foreach (var m in _lstMenu)
            {
                //4->opción Usuarios
                if (m.idMenu == 4)
                    busca = m.idMenu;
            }

            if (busca > 0)
            {
                valida = true;
            }

            return valida;
        }

        [HttpPost]
        public string validaMailToken(string Token)
        {
            string resp = string.Empty;
            try
            {
                if (_user.Token == Token)
                {
                    _user.IsTokenMail = 1;
                    resp = "OK";
                }
                else
                {
                    resp = "Código de validación incorrecto.";
                    _user.IsTokenMail = 0;
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en validación del Token enviado por Mail \n" + Token + "\n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }
    }
}

