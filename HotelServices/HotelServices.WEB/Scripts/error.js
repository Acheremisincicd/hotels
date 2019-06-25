$(document).ready(function () {
    $('form').on('submit', function () {
        $('strong').remove();
        if (!$('#tR').val()) {
            $('p').append("<strong style='color: red;'>Введите текст отзыва!</strong>");
            return false;
        }
        if ($('#tR').val().length < 6) {
            $('p').append("<strong style='color: red;'>Отзыв не может содержать меньше 6 символов!</strong>");
            return false;
        }
    });
})