function setUser() {
    var rName = $('#ContentPlaceHolder1_txtNombre').val().toLowerCase();
    var rPaterno = $('#ContentPlaceHolder1_txtPaterno').val().toLowerCase();
    var rMaterno = $('#ContentPlaceHolder1_txtMaterno').val().toLowerCase();
    $('#ContentPlaceHolder1_txtUser').val(rName.substring(0, 1) + rPaterno + rMaterno.substring(0, 1));
}

function valUser() {
    PageMethods.ValidateUser($('#ContentPlaceHolder1_txtUser').val(), onSucceed, onError);
}

// On Success
function onSucceed(results, currentContext, methodName) {
    if (results !== null) {
        if (results == 1) {
            warningMessage('Advertencia', 'El nombre de usuario ya se encuentra registrado en sistema. Favor de validar', 'ContentPlaceHolder1_txtUser');
            $('#ContentPlaceHolder1_txtUser').val('');
        }
    }
}

// On Error
function onError(results, currentContext, methodName) {
    errorMessage('Error', 'Ocurrio un error al validar el nombre de usuario. Favor de comunicarlo con el Administrador.');
}


//== Sweetalerts
function warningMessage(uxtitle, uxmessage, control) {
    swal(uxtitle, uxmessage, {
        icon: "warning",
        buttons: {
            confirm: {
                className: "btn btn-warning",
            },
        },
    }).then(function () {
        $("#" + control).focus();
    });
}

function errorMessage(uxtitle, uxmessage) {
    swal(uxtitle, uxmessage, {
        icon: "error",
        buttons: {
            confirm: {
                className: "btn btn-danger",
            },
        },
    });
}

function successMessage(uxtitle, uxmessage) {
    swal(uxtitle, uxmessage, {
        icon: "success",
        buttons: {
            confirm: {
                className: "btn btn-success",
            },
        },
    });
}

function infoMessage(uxtitle, uxmessage) {
    swal(uxtitle, uxmessage, {
        icon: "info",
        buttons: {
            confirm: {
                className: "btn btn-info",
            },
        },
    });
}

function successMessageRedirect(uxtitle, uxmessage, page) {
    swal(uxtitle, uxmessage, {
        icon: "success",
        buttons: {
            confirm: {
                className: "btn btn-success",
            },
        },
        timer: 20000,
    }).then(function () {
        window.location = page;
    });
}

function confirmMessage(status, usuario) {
    var bandera = status;
    var mensaje = "";
    if (bandera == "1")
        mensaje = "desbloquear";
    else
        mensaje = "bloquear";
    swal({
        title: "Está a punto de " + mensaje + " el usuario " + usuario,
        text: "¿Desea Continuar?",
        type: "warning",
        buttons: {
            confirm: {
                text: "Confirmar",
                className: "btn btn-success",
            },
            cancel: {
                text: "Cancelar",
                visible: true,
                className: "btn btn-danger",
            },
        },
    }).then((Delete) => {
        if (Delete) {
            PageMethods.changeUserStatus(status, usuario, _onSucceed, _onError);
        } else {
            swal.close();
        }
    });
}

// On Success
function _onSucceed(results, currentContext, methodName) {
    if (results !== null) {
        if (results == 1) {
            //successMessage('Éxito', 'Usuario actualizado con éxito.');
            successMessageRedirect('Éxito', 'Usuario actualizado con éxito.', 'Usuarios.aspx');
        }
    }
}

// On Error
function _onError(results, currentContext, methodName) {
    errorMessage('Error', 'Ocurrio un error al actualizar el estatus de usuario. Favor de comunicarlo con el Administrador.');
}

