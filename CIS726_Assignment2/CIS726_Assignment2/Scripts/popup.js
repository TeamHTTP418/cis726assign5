$(function () {
    $('#savecourse').bind('click', function (event) {
        event.preventDefault();
        var postdata = 'ID=' + $('#ID').val() + '&courseID=' + $('#courseID').val() + '&notes=' + $('#notes').val();
        //alert(postdata);
        $.ajax({
            url: '/Plans/SaveCourseInfo',
            data: postdata,
            type: 'POST',
            success: function (data) {
                $('#pcourseok').show();
                $('#pcoursebad').hide();
            },
            failure: function (data) {
                $('#pcoursebad').show();
                $('#pcourseok').hide();
            }
        });
    });
});