// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    if (!window.DataTable) {
        return;
    }

    document.querySelectorAll('.js-data-table').forEach(function (table) {
        if (DataTable.isDataTable(table)) {
            return;
        }

        new DataTable(table, {
            pageLength: 10,
            lengthMenu: [5, 10, 25, 50],
            language: {
                emptyTable: 'Nenhum registro encontrado',
                info: 'Mostrando _START_ até _END_ de _TOTAL_ registros',
                infoEmpty: 'Mostrando 0 até 0 de 0 registros',
                infoFiltered: '(filtrado de _MAX_ registros no total)',
                lengthMenu: 'Mostrar _MENU_ registros',
                loadingRecords: 'Carregando...',
                processing: 'Processando...',
                search: 'Buscar:',
                zeroRecords: 'Nenhum registro correspondente encontrado',
                paginate: {
                    first: 'Primeiro',
                    last: 'Último',
                    next: 'Próximo',
                    previous: 'Anterior'
                }
            },
            columnDefs: [
                {
                    targets: -1,
                    orderable: false,
                    searchable: false
                }
            ]
        });
    });
});
