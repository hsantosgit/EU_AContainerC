
function popMessage(xtitle, xmessage, xicon, xclass) {
    swal(xtitle, xmessage, {
        icon: xicon,
        buttons: {
            confirm: {
                className: xclass
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
        timer: 20000,
    }).then(function () {
        window.location = "InicioSesion.aspx"
    });
}

function togglePassword(obj) {
    obj.type = obj.type == 'password' ? 'text' : 'password';
}


$(function () {
    var mayus = new RegExp("^(?=.*[A-Z])");
    var special = new RegExp("^(?=.*[!@#$%&*])");
    var numbers = new RegExp("^(?=.*[0-9])");
    var lower = new RegExp("^(?=.*[a-z])");
    var len = new RegExp("^(?=.{8,})");

    var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%&*])(?=.{15,})");
    var highgRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%&*])(?=.{20,})");

    var regExp = [mayus, special, numbers, lower, len];
    var elementos = [$("#mayus"), $("#special"), $("#numbers"), $("#lower"), $("#len")];

    $("#txtPass").on("keyup", function () {
        var pass = $("#txtPass").val();
        var check = 0;
        var _uxclass = $('#progressdiv').attr('class');
        for (var i = 0; i < 5; i++) {
            if (regExp[i].test(pass)) {
                elementos[i].hide();
                check++;
            } else {
                elementos[i].show();
            }
        }

        if (check == 0) {
            $('#progressdiv').removeClass(_uxclass).addClass('progress-bar w-0');
            $("#mensaje").text("");
        } else if (check >= 0 && check <= 4) {
            $('#progressdiv').removeClass(_uxclass).addClass('progress-bar bg-danger w-25');
            $("#mensaje").text("Muy insegura").css("color", "red");
        } else if (check == 5) {
            $('#progressdiv').removeClass(_uxclass).addClass('progress-bar bg-warning w-50');
            $("#mensaje").text("Poco Segura").css("color", "orange");
            if (highgRegex.test($(this).val())) {
                $('#progressdiv').removeClass('progress-bar bg-warning w-75').addClass('progress-bar bg-success w-100');
                $("#mensaje").text("Muy Segura").css("color", "green");
            } else if (strongRegex.test($(this).val())) {
                $('#progressdiv').removeClass('progress-bar bg-warning w-50').addClass('progress-bar bg-success w-75');
                $("#mensaje").text("Segura").css("color", "green");
            }
        }
    });
});


function mostrarPassword(control) {
    var cambio = document.getElementById(control);

    if (cambio.type == "password") {
        cambio.type = "text";
        $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
    } else {
        cambio.type = "password";
        $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
    }
}




