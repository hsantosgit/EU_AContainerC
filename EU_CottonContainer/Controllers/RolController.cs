using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    public class RolController : Controller
    {
        private static Usuario _user;
        private static List<Rol> _lstRol = new List<Rol>();
        // GET: RolController
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

            _lstRol = RolFacade.ObtenRol();
            return View(_lstRol);
        }

        public ActionResult AddRol()
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

        public ActionResult EditRol(int _idRol)
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

            Rol _rol = new Rol();
            foreach (Rol rol in _lstRol.FindAll(x => x.idRol == _idRol))
            {
                _rol = rol;
            }
            return View(_rol);
        }

        [HttpPost]
        public string SaveRol(Rol _rol)
        {
            string resp = string.Empty;
            try
            {
                if (RolFacade.AddRol(_rol) > 0)
                {
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 6, Accion = "Nuevo registro, Rol: " + _rol.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "OK";
                }
                else
                {
                    resp = GlobalConfiguration.ExceptionError;
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de Rol \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public string SaveEditRol(Rol _rol)
        {
            string resp = string.Empty;
            try
            {
                if (RolFacade.EditRol(_rol) > 0)
                {
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 6, Accion = "Edición registro, Rol: " + _rol.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
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

        public IActionResult PerfilRolPartial(int _iRol)
        {
            List<Perfil> _listPerfil = new List<Perfil>();
            _listPerfil = PerfilFacade.ObtenPerfil();

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            nodes.Add(new TreeViewNode { id = "0", parent = "#", text = "Perfiles" });
            foreach (var child in _listPerfil)
            {
                State checkStateson = new State();
                checkStateson.selected = SelectedNodes(_iRol).Contains(child.idPerfil.ToString()) ? true : false;
                nodes.Add(new TreeViewNode { id = "0-" + child.idPerfil.ToString(), parent = "0", text = child.Nombre, state = checkStateson });
            }
            Rol uxrol = new Rol();
            uxrol.Nodes = JsonConvert.SerializeObject(nodes);
            uxrol.idRol = _iRol;
            foreach (var r in _lstRol.FindAll(x => x.idRol == _iRol))
            {
                uxrol.Nombre = r.Nombre;
            }
            return PartialView("_PerfilRol", uxrol);
        }

        [HttpPost]
        public string SavePerfilRol(string selectedItems, Rol _rol)
        {
            string resp = string.Empty;

            if (!String.IsNullOrEmpty(selectedItems))
            {
                List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                //Eliminar Perfiles actuales del Rol e ingresar los nuevos
                RolFacade.DelRolOptions(_rol);
                _rol.idNivel = 0;
                _rol.Nombre = _user.userName;
                foreach (var i in items)
                {
                    _rol.idPerfil = Convert.ToInt32(i.id);
                    RolFacade.AddRolPerfil(_rol);
                }

                resp = "OK";
            }
            else
            {
                resp = "Debe seleccionar al menos una opción de menú para agregar al perfil.";
            }

            return resp;
        }

        public List<string> SelectedNodes(int _iRol)
        {
            List<string> nodes = new List<string>();
            List<Perfil> _listPerfilBy = new List<Perfil>();
            _listPerfilBy = PerfilFacade.ObtenPerfilBy(_iRol);
            foreach (var node in _listPerfilBy)
            {
                nodes.Add(node.idPerfil.ToString());
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
                //6->opción Perfil
                if (m.idMenu == 6)
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
