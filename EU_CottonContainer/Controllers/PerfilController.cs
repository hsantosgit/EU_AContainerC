using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private static Usuario _user;
        private static int uxIdPadre = 0;
        private static List<Perfil> _lstperfil;
        // GET: PerfilController
        public ActionResult Index()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }

            _lstperfil = PerfilFacade.ObtenPerfil();
            return View(_lstperfil);
        }

        public ActionResult AddPerfil()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult EditPerfil(int _idPerfil)
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }
            Perfil _perf = new Perfil();
            foreach (Perfil perfil in _lstperfil.FindAll(x => x.idPerfil == _idPerfil))
            {
                _perf = perfil;
            }
            return View(_perf);
        }

        [HttpPost]
        public string SaveEditPerfil(Perfil _perfil)
        {
            string resp = string.Empty;
            try
            {
                if (PerfilFacade.EditPerfil(_perfil) > 0)
                {

                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 7, Accion = "Edición registro, Perfil: " + _perfil.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "OK";
                }
                else
                {
                    resp = GlobalConfiguration.ExceptionError;
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en edición de Perfil \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        public ActionResult EditPerfilOptions(string _sPerfil)
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }

            List<Perfil> _lstperfil = PerfilFacade.ObtenPerfil();
            return View("Index", _lstperfil);
        }

        public IActionResult MenuPerfilPartial(string _sPerfil)
        {
            List<Menu> _lstmenu = MenuFacade.ObtenerMenu();
            List<Menu> _lstmenuRol = MenuFacade.ObtenerMenuPerfil(new Menu { idPerfil = Convert.ToInt32(_sPerfil) });
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            foreach (var node in _lstmenu.FindAll(x => x.idPadre == 0))
            {
                nodes.Add(new TreeViewNode { id = node.idMenu.ToString(), parent = "#", text = node.Nombre });
                foreach (var child in _lstmenu.FindAll(x => x.idPadre == node.idMenu && x.idPadre > 0))
                {
                    State checkStateson = new State();
                    checkStateson.selected = SelectedNodes(_sPerfil).Contains(child.idMenu.ToString()) ? true : false;
                    nodes.Add(new TreeViewNode { id = child.idPadre.ToString() + "-" + child.idMenu.ToString(), parent = child.idPadre.ToString(), text = child.Nombre, state = checkStateson });
                }
            }
            Perfil per = new Perfil();
            per.Nodes = JsonConvert.SerializeObject(nodes);
            per.idPerfil = Convert.ToInt32(_sPerfil);
            foreach (var node in _lstperfil.FindAll(x => x.idPerfil == Convert.ToInt32(_sPerfil)))
            {
                per.MenuPadre = node.Nombre;
            }

            return PartialView("_MenuPerfil", per);
        }

        [HttpPost]
        public string SaveMenuPerfil(string selectedItems, Perfil _perfil)
        {
            string resp = string.Empty;

            if (!String.IsNullOrEmpty(selectedItems))
            {
                List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                //Eliminar Permisos actuales del perfil e ingresar los nuevos
                PerfilFacade.DelUserOptions(_perfil);
                _perfil.MenuPadre = _user.userName;
                uxIdPadre = 0;
                foreach (var i in items)
                {
                    _perfil.IdMenu = Convert.ToInt32(i.id);
                    PerfilFacade.AddPerfilMenu(_perfil);
                    
                    if (i.parent != "0")
                    {
                        _perfil.IdMenu = Convert.ToInt32(i.parent);
                        if (uxIdPadre == 0)
                            PerfilFacade.AddPerfilMenu(_perfil);

                        uxIdPadre += 1;
                        //Se guarda el nodo padre

                    }
                }

                resp = "OK";
            }
            else
            {
                resp = "Debe seleccionar al menos una opción de menú para agregar al perfil.";
            }

            return resp;
        }

        [HttpPost]
        public string SavePerfil(Perfil _perfil)
        {
            string resp = string.Empty;
            try
            {
                if (PerfilFacade.AddPerfil(_perfil) > 0)
                {

                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 7, Accion = "Nuevo registro, Perfil: " + _perfil.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "OK";
                }
                else
                {
                    resp = GlobalConfiguration.ExceptionError;
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de Perfil \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        public List<string> SelectedNodes(string sperfil)
        {
            List<string> nodes = new List<string>();
            List<Menu> _lstmenu = new List<Menu>();
            Menu _menu = new Menu();
            _menu.idPerfil = Convert.ToInt32(sperfil);
            _lstmenu = MenuFacade.ObtenerMenuPerfil(_menu);
            foreach (var node in _lstmenu)
            {
                nodes.Add(node.idMenu.ToString());
            }
            return nodes;
        }

        private bool ValidaAccesoUsuario()
        {
            bool valida = false;
            Menu _menu = new Menu { idRol = _user.idRol };
            List<Menu> _lstMenu = MenuFacade.ObtenerMenuRol(_menu);
            int busca = 0;
            foreach (var m in _lstMenu)
            {
                //7->opción Perfil
                if (m.idMenu == 7)
                    busca = m.idMenu;
            }

            if (busca > 0)
            {
                valida = true;
            }

            return valida;
        }

    }
}
