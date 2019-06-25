$(document).ready(function () {
    $('#toSort').click(
        function () {
            $('.sort').slideToggle(500, function () {
                if ($('#toSort').html() == 'Сортировка🡇') {
                    $('#toSort').html('Сортировка🡅');
                }
                else $('#toSort').html('Сортировка🡇');
            }
            )
            return false;
        }
    )
});