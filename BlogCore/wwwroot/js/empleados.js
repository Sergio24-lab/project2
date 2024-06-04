var dataTable;
$(document).ready(function () {
    cargarDatatable();

    $("#btnGenerarReporte").on("click", function () {
        generarReporte();
    });
});

function cargarDatatable() {
    dataTable = $("#tblEmpleados").DataTable({
        "ajax": {
            "url": "/Admin/Empleado/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "20%" },
            { "data": "fechaNacimiento", "width": "10%" },
            { "data": "salario", "width": "10%" },
            { "data": "direccion", "width": "10%" },
            { "data": "correoElectronico", "width": "15%" },
            { "data": "numeroTelefono", "width": "10%" },
            { "data": "fechaContratacion", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Empleado/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick="Delete(${data})" class="btn btn-danger text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-trash-alt"></i> Borrar
                                </a>
                            </div>`;
                }, "width": "12%"
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}

function Delete(id) {
    Swal.fire({
        title: "¿Está seguro de borrar?",
        text: "Este contenido no se puede recuperar.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Sí, borrar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: '/Admin/Empleado/Delete/' + id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    toastr.error("Error al intentar borrar el empleado.");
                }
            });
        }
    });
}

function generarReporte() {
    $.ajax({
        type: "POST",
        url: "/Admin/Empleado/GenerarReporte",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                window.location.href = data.pdfUrl;
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            toastr.error("Error al generar el reporte.");
        }
    });
}
