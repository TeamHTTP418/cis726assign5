$(function () {
    //This toggles the filter box on the main index page
    $('.toggle_me').click(function(){
        $(this).siblings().slideToggle("slow");
        $(this).children('.img_switch').toggle();
    });

    //This determines if the filter box should be open or closed on page load
    var myVal = $('.toggle_me').data("filtered");
    if (myVal == "False") {
        $('.toggle_me').siblings().toggle();
        $('.toggle_me').children('.img_switch').toggle();
    }

    // This will add a required course row to the Degree Programs form
    $("#addRequiredCourse").unbind('click');
    $("#addRequiredCourse").on('click', function (event) {
        event.preventDefault();
        var myVal = $('#requiredcourses').data("degreeprogramid");
        //todo Change me when deploy if base URL is different
        $.get('/DegreePrograms/RequiredCourseRow/' + myVal, function (template) {
            $("#requiredcourses").append(template);
            $("#requiredcourses").find('.reqcourseauto').autocomplete({
                minLength: 2,
                source: function (req, resp) {
                    $.ajax({
                        //todo Change me when deploy if base URL is different
                        url: "/Courses/SearchCourses",
                        type: "POST",
                        dataType: "json",
                        data: { term: req.term },
                        success: function (data) {
                            resp($.map(data, function (item) {
                                return { label: item.courseHeader, value: item.ID };
                            }));
                        }
                    });
                },
                select: function (event, ui) {
                    var id = ui.item.value;
                    var name = ui.item.label;
                    $(this).siblings('.reqcourseid').val(id);
                    // update what is displayed in the textbox
                    this.value = name;
                    return false;
                },
                messages: {
                    noResults: '',
                    results: function () { }
                }
            });
        });
        
    });

    //This will add an elective course to the Degree Programs form
    $("#addElectiveCourse").unbind('click');
    $("#addElectiveCourse").on('click', function (event) {
        event.preventDefault();
        var myVal = $('#electivecourses').data("degreeprogramid");
        //todo Change me when deploy if base URL is different
        $.get('/DegreePrograms/ElectiveCourseRow/' + myVal, function (template) {
            $("#electivecourses").append(template);
            $("#electivecourses").find(".eleccourseauto").autocomplete({
                minLength: 2,
                source: function (req, resp) {
                    $.ajax({
                        //todo Change me when deploy if base URL is different
                        url: "/ElectiveLists/SearchElectiveLists",
                        type: "POST",
                        dataType: "json",
                        data: { term: req.term },
                        success: function (data) {
                            resp($.map(data, function (item) {
                                return { label: item.electiveListName, value: item.ID };
                            }));
                        }
                    });
                },
                select: function (event, ui) {
                    var id = ui.item.value;
                    var name = ui.item.label;
                    $(this).siblings('.eleccourseid').val(id);
                    // update what is displayed in the textbox
                    this.value = name;
                    return false;
                },
                messages: {
                    noResults: '',
                    results: function () { }
                }
            });
        });
    });

    //This will add a course row to the Elective List form
    $("#addElectiveListCourse").unbind('click');
    $("#addElectiveListCourse").on('click', function (event) {
        event.preventDefault();
        var myVal = $('#electivelistcourses').data("electivelistid");
        //todo Change me when deploy if base URL is different
        $.get('/ElectiveLists/ElectiveListCourseRow/' + myVal, function (template) {
            $("#electivelistcourses").append(template);
            $("#electivelistcourses").find(".eleclistcourseauto").autocomplete({
                minLength: 2,
                source: function (req, resp) {
                    $.ajax({
                        //todo Change me when deploy if base URL is different
                        url: "/Courses/SearchCourses",
                        type: "POST",
                        dataType: "json",
                        data: { term: req.term },
                        success: function (data) {
                            resp($.map(data, function (item) {
                                return { label: item.courseHeader, value: item.ID };
                            }));
                        }
                    });
                },
                select: function (event, ui) {
                    var id = ui.item.value;
                    var name = ui.item.label;
                    $(this).siblings('.eleclistcourseid').val(id);
                    // update what is displayed in the textbox
                    this.value = name;
                    return false;
                },
                messages: {
                    noResults: '',
                    results: function () { }
                }
            });
        });
    });

    //This will add a course row to the Elective List form
    $("#addPrerequisiteCourse").unbind('click');
    $("#addPrerequisiteCourse").on('click', function (event) {
        event.preventDefault();
        var myVal = $('#prereqcourses').data("courseid");
        //todo Change me when deploy if base URL is different
        $.get('/Courses/PrerequisiteCourseRow/' + myVal, function (template) {
            $("#prereqcourses").append(template);
            $("#prereqcourses").find(".prereqcourseauto").autocomplete({
                minLength: 2,
                source: function (req, resp) {
                    $.ajax({
                        //todo Change me when deploy if base URL is different
                        url: "/Courses/SearchCourses",
                        type: "POST",
                        dataType: "json",
                        data: { term: req.term },
                        success: function (data) {
                            resp($.map(data, function (item) {
                                return { label: item.courseHeader, value: item.ID };
                            }));
                        }
                    });
                },
                select: function (event, ui) {
                    var id = ui.item.value;
                    var name = ui.item.label;
                    $(this).siblings('.prereqcourseid').val(id);
                    // update what is displayed in the textbox
                    this.value = name;
                    return false;
                },
                messages: {
                    noResults: '',
                    results: function () { }
                }
            });
        });
    });

    //This does the autocomplete for the required courses on the Degree Programs form
    $(".reqcourseauto").autocomplete({
        minLength: 2,
        source: function (req, resp) {
            $.ajax({
                //todo Change me when deploy if base URL is different
                url: "/Courses/SearchCourses",
                type: "POST",
                dataType: "json",
                data: { term: req.term },
                success: function (data) {
                    resp($.map(data, function (item) {
                        return { label: item.courseHeader, value: item.ID };
                    }));
                }
            });
        },
        select: function (event, ui) {
            var id = ui.item.value;
            var name = ui.item.label;
            $(this).siblings('.reqcourseid').val(id);
            // update what is displayed in the textbox
            this.value = name;
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });

    //This does the autocomplete for the elective lists on the Degree Programs form
    $(".eleccourseauto").autocomplete({
        minLength: 2,
        source: function (req, resp) {
            $.ajax({
                //todo Change me when deploy if base URL is different
                url: "/ElectiveLists/SearchElectiveLists",
                type: "POST",
                dataType: "json",
                data: { term: req.term },
                success: function (data) {
                    resp($.map(data, function (item) {
                        return { label: item.electiveListName, value: item.ID };
                    }));
                }
            });
        },
        select: function (event, ui) {
            var id = ui.item.value;
            var name = ui.item.label;
            $(this).siblings('.eleccourseid').val(id);
            // update what is displayed in the textbox
            this.value = name;
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });

    //This does the autocomplete on the course list on the Elective List form
    $(".eleclistcourseauto").autocomplete({
        minLength: 2,
        source: function (req, resp) {
            $.ajax({
                //todo Change me when deploy if base URL is different
                url: "/Courses/SearchCourses",
                type: "POST",
                dataType: "json",
                data: { term: req.term },
                success: function (data) {
                    resp($.map(data, function (item) {
                        return { label: item.courseHeader, value: item.ID };
                    }));
                }
            });
        },
        select: function (event, ui) {
            var id = ui.item.value;
            var name = ui.item.label;
            $(this).siblings('.eleclistcourseid').val(id);
            // update what is displayed in the textbox
            this.value = name;
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });

    //This does the autocomplete on the prerequisite course list on the Course form
    $(".prereqcourseauto").autocomplete({
        minLength: 2,
        source: function (req, resp) {
            $.ajax({
                //todo Change me when deploy if base URL is different
                url: "/Courses/SearchCourses",
                type: "POST",
                dataType: "json",
                data: { term: req.term },
                success: function (data) {
                    resp($.map(data, function (item) {
                        return { label: item.courseHeader, value: item.ID };
                    }));
                }
            });
        },
        select: function (event, ui) {
            var id = ui.item.value;
            var name = ui.item.label;
            $(this).siblings('.prereqcourseid').val(id);
            // update what is displayed in the textbox
            this.value = name;
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });
});

    