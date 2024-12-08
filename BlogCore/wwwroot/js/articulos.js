var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblArticulos").DataTable({
        "ajax": {
            "url": "/Admin/Articulos/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "15%" },
            { "data": "categoria.nombre", "width": "10%" },
            {
                "data": "urlImagen",
                "render": function (imagen) {
                    return `<img src="../${imagen}" width="80">`;
                }
            },
            { "data": "fechaCreacion", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Articulos/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick="Delete('/Admin/Articulos/Delete/${data}')" class="btn btn-danger text-white" style="cursor:pointer; width:100px; height:40px">
                                    <i class="far fa-trash-alt"></i> Borrar
                                </a>
                                &nbsp;
                                <a onclick="Comprar(${data})" class="btn btn-primary text-white" style="cursor:pointer; width:120px; height:40px">
                                    <i class="fas fa-shopping-cart"></i>Comprar
                                </a>
                            </div>`;
                }, "width": "80%"
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

function Delete(url) {
    Swal.fire({
        title: "¿Está seguro de que desea borrar?",
        text: "¡Este contenido no se puede recuperar!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Sí, borrar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function Comprar(id) {
    Swal.fire({
        title: 'Comprar Artículo',
        html: `
           <div>
                <div style="margin-bottom: 1rem;">
                    <label for="cantidad" style="display: block; margin-bottom: 0.5rem;">Cantidad:</label>
                    <input id="cantidad" type="number" min="1" placeholder="Cantidad" required class="swal2-input" style="width: 100%;">
                </div>
                <div>
                    <label for="precio" style="display: block; margin-bottom: 0.5rem;">Precio:</label>
                    <input id="precio" type="number" min="0" step="0.01" placeholder="Precio" required class="swal2-input" style="width: 100%;">
                </div>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'Comprar',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            const cantidad = document.getElementById('cantidad').value;
            const precio = document.getElementById('precio').value;
            if (!cantidad || cantidad <= 0) {
                Swal.showValidationMessage('Por favor, ingrese una cantidad válida');
                return false;
            }
            if (!precio || precio <= 0) {
                Swal.showValidationMessage('Por favor, ingrese un precio válido');
                return false;
            }
            return $.ajax({
                type: 'POST',
                url: `/Admin/Articulos/Comprar`,
                data: JSON.stringify({ id: id, cantidad: cantidad, precio: precio }),
                contentType: 'application/json',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                        Swal.close();
                    } else {
                        toastr.error(data.message);
                        Swal.showValidationMessage(data.message);
                    }
                },
                error: function (error) {
                    Swal.showValidationMessage(`Request failed: ${error}`);
                }
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}
