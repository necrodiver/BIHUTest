$(document).ready(function () {
    $('.ui.dropdown').dropdown({//.ui.menu 
        on: 'hover'
    });
    $('.ui.menu a.item')
        .on('click', function () {
            $(this)
                .addClass('active')
                .siblings()
                .removeClass('active');
        });
    $('.combo.dropdown')
        .dropdown({
            action: 'combo'
        });
});