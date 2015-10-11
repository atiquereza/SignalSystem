/**
 * Created by Sakib on 10/7/2015.
 */
$(function () {
    $("#datetimepicker6").datepicker({
        todayBtn:  1,
        autoclose: true
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#enddate').datepicker('setStartDate', minDate);
    });

    $("#datetimepicker7").datepicker({
        todayBtn:  1,
        autoclose: true
    }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#startdate').datepicker('setEndDate', minDate);
        });

});
