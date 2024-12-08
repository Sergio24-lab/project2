$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblCategorias").DataTable({
        "ajax": {
            "url": "/admin/categorias/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "40%" },
            { "data": "orden", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Categoria/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick="Delete(${data})" class="btn btn-danger text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-trash-alt"></i> Borrar
                                </a>
                                
                            </div>`;
                }, "width": "40%"
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
                url: '/Admin/Categorias/Delete/' + id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    toastr.error("Error al intentar borrar la categoría.");
                }
            });
        }
    });
}
