// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Carga cualquier partial dentro del modal compartido.
// Las pantallas futuras solo necesitan usar la clase js-modal-link.
$(document).on('click', '.js-modal-link', function (event) {
    event.preventDefault();

    const link = $(this);
    const modal = $('#sharedModal');
    const modalTitle = $('#sharedModalTitle');
    const modalBody = $('#sharedModalBody');

    modalTitle.text(link.data('modal-title') || 'Details');
    modalBody.html('<div class="text-center">Loading...</div>');

    // Bootstrap 5 administra el ciclo de vida del modal mediante su API JavaScript.
    bootstrap.Modal.getOrCreateInstance(modal[0]).show();

    $.get(link.attr('href'))
        .done(function (html) {
            modalBody.html(html);
        })
        .fail(function () {
            modalBody.html('<div class="alert alert-danger mb-0">The information could not be loaded.</div>');
        });
});

// Envía los formularios de los modales sin abandonar el índice.
$(document).on('submit', '.js-modal-form', function (event) {
    event.preventDefault();

    const form = $(this);
    const modalBody = $('#sharedModalBody');

    $.ajax({
        url: form.attr('action'),
        type: form.attr('method') || 'POST',
        data: form.serialize()
    })
        .done(function (response) {
            if (response && response.success === true) {
                window.location.reload();
                return;
            }

            // Si hay errores de validación, el servidor devuelve nuevamente la partial.
            modalBody.html(response);
        })
        .fail(function () {
            modalBody.prepend('<div class="alert alert-danger">The operation could not be completed.</div>');
        });
});
