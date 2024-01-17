function ConsultarNombre() {
    let identificacion = $("#Identificacion").val();
    if (identificacion.length > 0) {
        $.ajax({
            type: "GET",
            url: "https://apis.gometa.org/cedulas/" + identificacion,
            dataType: "json",
            success: function (result) {
                $("#NombreCompleto").val(result.nombre);
            }
        });
    }
    else {
        $("#NombreCompleto").val("");
    }
}