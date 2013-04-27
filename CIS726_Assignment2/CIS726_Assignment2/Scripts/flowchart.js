$(function () {

    //Canvas and drawing context
    var canvas = $('#flowchartCanvas')[0]
    var ctx = canvas.getContext('2d');

    //arrays for courses and semesters
    var courseList = [];
    var semesterList = [];

    //flowchart anchor points (associative array)
    var anchorCols = [];
    var anchorRows = [];
	
    //Offset of semesters in list so we can look up semesters by ID
	var semesterOffset = -1;

    //size of boxes on flowchart
	var boxwidth = 80;
	var boxheight = 60;
	var boxgap = 39;

    //height of Semester boxes at the top and the gap left
	var topheight = 30;
	var topgap = topheight + boxgap;

    //Gap on left side for prereq arrows
	var leftgap = boxgap;

    //index of the course that has been clicked on
	var clickedIndex = -1;

    //maximum number of columns to constrain dragging
	var maxCol = -1;

    //number of anchor points for flowchart arrows
	var anchorGap = 3;
	var numAnchors = (boxgap / anchorGap) - 2;

    //max number of courses per semester on flowchart
	var maxCoursePerSem = 7;

	function clear() {
	    // Store the current transformation matrix
	    ctx.save();

	    // Use the identity matrix while clearing the canvas
	    ctx.setTransform(1, 0, 0, 1, 0, 0);
	    ctx.clearRect(0, 0, canvas.width, canvas.height);

	    // Restore the transform
	    ctx.restore();
	}

	function redraw() {
	    ctx.beginPath();
	    ctx.moveTo(0, topheight + (boxgap / 2));
	    ctx.lineTo(canvas.width, topheight + (boxgap / 2));
	    ctx.stroke();
	    semesterList.forEach(function(semester) {
	        drawSemester(semester);
	    })
	    courseList.forEach(function(course) {
	        drawCourse(course);
	    })
	    drawPrereqArrows();
	}

	function Course(pcourseID, courseID, courseTitle, courseName, elistID, elistName, semester, order, hours, prereq) {
	    this.pcourseID = pcourseID;
		this.courseID = courseID;
		this.courseTitle = courseTitle;
		this.courseName = courseName;
		this.elistID = elistID;
		this.elistName = elistName;
		this.semester = semester;
		this.order = order;
		this.hours = hours;
		this.prereq = prereq;
		this.x = 0;
		this.y = 0;
    }
	
	function Semester(semesterID, semesterName, show, col) {
		this.semesterID = semesterID;
		this.semesterName = semesterName;
		this.show = show;
		this.col = col;
	}

	function addSemester(sem) {
	    semesterList[sem - semesterOffset].show = 'true';
	    $("#semesterID").find('option[value=' + sem + ']').remove();
	    var col = 0;
	    semesterList.forEach(function (semester, index) {
	        if (semester.show == 'true') {
	            semester.col = col;
	            col++;
	        } else {
	            semester.col = -1;
	        }
	    });
	    maxCol = col;
	    if (maxCol > -1) {
	        canvas.width = leftgap + (maxCol) * (boxwidth + boxgap) + boxgap;
	    } else {
	        canvas.width = leftgap + boxwidth + boxgap;
	    }
	}

	function drawDropZone(x, y) {
	    var i = 0;
	    var j = 0;
	    while ((i + 1) * (boxwidth + boxgap) + leftgap < x) {
	        i++
	    }
	    while ((j + 1) * (boxheight + boxgap) + topgap < y) {
	        j++;
	    }
	    if (i < maxCol && j < maxCoursePerSem) {
	        var dx = i * (boxwidth + boxgap) + leftgap;
	        var dy = j * (boxheight + boxgap) + topgap;
	        ctx.fillStyle = "#FFFF99";
	        ctx.fillRect(dx, dy, boxwidth, boxheight);
	        ctx.fillStyle = "#000000";
	    }
	}

	function dropCourse(x, y) {
	    var i = 0;
	    var j = 0;
	    while ((i + 1) * (boxwidth + boxgap) + leftgap < x) {
	        i++
	    }
	    while ((j + 1) * (boxheight + boxgap) + topgap < y) {
	        j++;
	    }
	    if (j < maxCoursePerSem) {
	        var dx = i * (boxwidth + boxgap) + leftgap;
	        var dy = j * (boxheight + boxgap) + topgap;
	        var newSemester = -1;
	        var oldSemester = courseList[clickedIndex].semester;
	        var oldOrder = courseList[clickedIndex].order;
	        semesterList.forEach(function (semester, index) {
	            if (semester.col == i) {
	                newSemester = semester.semesterID;
	            }
	        });
	        if (newSemester > -1 && (newSemester != oldSemester || j != oldOrder)) {
	            var numSemester = 0;
	            courseList.forEach(function (course, index) {
	                if (course.semester == newSemester) {
	                    numSemester++;
	                }
	            });
	            if (numSemester < maxCoursePerSem) {
	                courseList.forEach(function (course, index) {
	                    if (course.semester == oldSemester && course.order >= oldOrder) {
	                        course.order--;
	                    }
	                    if (course.semester == newSemester && course.order >= j) {
	                        course.order++;
	                    }
	                });
	                courseList[clickedIndex].order = j;
	                courseList[clickedIndex].semester = newSemester;
	                var postdata = 'ID=' + courseList[clickedIndex].pcourseID + '&semester=' + newSemester + '&order=' + j;
	                //alert(postdata);
	                $.ajax({
	                    url: '/Plans/MoveCourse',
	                    data: postdata,
	                    type: 'POST',
	                    failure: function (data) {
	                        alert("Changes not saved!");
	                    }
	                });
	            }
	        }
	    }
	}

    function drawCourse(course) {
        var x = semesterList[course.semester - semesterOffset].col * (boxwidth + boxgap) + leftgap;
		var y = (boxheight + boxgap) * course.order + topgap;
		ctx.strokeRect(x, y, boxwidth, boxheight);
		if (course.courseID > 0) {
		    ctx.fillText(course.courseTitle + ' (' + course.hours + ')', x + 2, y + 10, boxwidth - 4);
		} else if (course.elistID > 0) {
		    ctx.fillText(course.elistName+ ' (' + course.hours + ')', x + 2, y + 10, boxwidth - 4);
		}
		course.x = x;
		course.y = y;
    }

    function drawCourseDrag(course, x, y) {
        ctx.fillStyle = "#B9A9CF";
        ctx.fillRect(x, y, boxwidth, boxheight);
        ctx.fillStyle = "#000000";
        ctx.strokeRect(x, y, boxwidth, boxheight);
        if (course.courseID > 0) {
            ctx.fillText(course.courseTitle + ' (' + course.hours + ')', x + 2, y + 10, boxwidth - 4);
        } else if (course.elistID > 0) {
            ctx.fillText(course.elistName + ' (' + course.hours + ')', x + 2, y + 10, boxwidth - 4);
        }
    }

    function drawSemester(semester) {
        if (semester.show == 'true') {
            var x = semester.col * (boxwidth + boxgap) + leftgap;
            var y = 0;
            ctx.strokeRect(x, y, boxwidth, topheight);
            ctx.fillText(semester.semesterName, x + 2, y + 10, boxwidth - 4);
            semester.x = x;
            semester.y = y;
        }
    }

    function drawPrereqArrows() {
        anchorCols = Array();
        anchorRows = Array();
        courseList.forEach(function (course, index) {
            if(course.prereq != null){
                course.prereq.forEach(function (item) {
                    var toCourseIdx = courseByID(item);
                    if (toCourseIdx > -1) {
                        var toCourse = courseList[toCourseIdx];
                        drawArrow(semesterList[course.semester - semesterOffset].col, course.order, semesterList[toCourse.semester - semesterOffset].col, toCourse.order);
                    }
                });
            }
        });
    }

    function courseByID(id) {
        var retVal = -1;
        courseList.forEach(function (course, index) {
            if (course.courseID == id) {
                retVal = index;
            }
        });
        return retVal;
    }

    function drawArrow(col2, row2, col1, row1) {
       if (col1 == col2) { //same column, so concurrent enrollment
            ctx.strokeStyle = "#0000FF";
            ctx.beginPath();
            var startPtx = col1 * (boxwidth + boxgap) + boxwidth + leftgap;
            var startPty = row1 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var endPtx = col2 * (boxwidth + boxgap) + leftgap;
            var endPty = row2 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var critPt1x = -1;
            var critPt1y = -1;
            var critPt2x = -1;
            col2 = col2 - 1;
            if (row1 < row2) { //going down
                startPty = startPty + anchorGap;
                endPty = endPty - (anchorGap * 2);
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = row1 * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                    anchorCols[col2] = 1;
                }
                anchorCols[col2]++;
                critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
            } else { //going up
                startPty = startPty - anchorGap;
                endPty = endPty + (anchorGap * 2);
                row1 = row1 - 1;
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = (row1) * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                    anchorCols[col2] = 1;
                }
                anchorCols[col2]++;
                critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
            }
            ctx.moveTo(startPtx, startPty);
            ctx.lineTo(critPt1x, startPty);
            ctx.lineTo(critPt1x, critPt1y);
            ctx.lineTo(critPt2x, critPt1y);
            ctx.lineTo(critPt2x, endPty);
            ctx.lineTo(endPtx, endPty);
            ctx.stroke();
            ctx.strokeStyle = "#000000";
        }else if (col1 < col2) { //standard arrows
            ctx.beginPath();
            var startPtx = col1 * (boxwidth + boxgap) + boxwidth + leftgap;
            var startPty = row1 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var endPtx = col2 * (boxwidth + boxgap) + leftgap;
            var endPty = row2 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var critPt1x = -1;
            var critPt1y = -1;
            var critPt2x = -1;
            col2 = col2 - 1;
            if (row1 < row2) { //going down
                startPty = startPty + anchorGap;
                endPty = endPty - (anchorGap * 2);
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = row1 * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (col1 < col2) { //we know we have to bend
                    if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                        anchorCols[col2] = 1;
                    }
                    anchorCols[col2]++;
                    critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
                } else {
                    critPt2x = critPt1x;
                }
            } else if (row1 > row2) { //going up
                startPty = startPty - anchorGap;
                endPty = endPty + (anchorGap * 2);
                row1 = row1 - 1;
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = (row1) * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (col1 < col2) { //we know we have to bend
                    if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                        anchorCols[col2] = 1;
                    }
                    anchorCols[col2]++;
                    critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
                } else {
                    critPt2x = critPt1x;
                }
            } else { //same row
                if (col1 < col2) { //we know we have to bend
                    startPty = startPty + anchorGap;
                    endPty = endPty + (anchorGap * 2);
                    if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                        anchorCols[col1] = 1;
                    }
                    if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                        anchorRows[row1] = 1;
                    }
                    anchorCols[col1]++;
                    anchorRows[row1]++;
                    critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                    critPt1y = row1 * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                    if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                        anchorCols[col2] = 1;
                    }
                    anchorCols[col2]++;
                    critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
                } else { //same row and next to each other, simple case!
                    critPt1x = startPtx;
                    critPt2x = startPtx;
                    critPt1y = startPty;
                }
            }
            ctx.moveTo(startPtx, startPty);
            ctx.lineTo(critPt1x, startPty);
            ctx.lineTo(critPt1x, critPt1y);
            ctx.lineTo(critPt2x, critPt1y);
            ctx.lineTo(critPt2x, endPty);
            ctx.lineTo(endPtx, endPty);
            ctx.stroke();
        }else if (col1 > col2) { //prerequisite not taken before needed class. THIS IS BAD!
            ctx.strokeStyle = "#FF0000";
            ctx.beginPath();
            var startPtx = col1 * (boxwidth + boxgap) + boxwidth + leftgap;
            var startPty = row1 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var endPtx = col2 * (boxwidth + boxgap) + leftgap;
            var endPty = row2 * (boxheight + boxgap) + topgap + (boxheight / 2);
            var critPt1x = -1;
            var critPt1y = -1;
            var critPt2x = -1;
            col2 = col2 - 1;
            if (row1 < row2) { //going down
                startPty = startPty + anchorGap;
                endPty = endPty - (anchorGap * 2);
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = row1 * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                    anchorCols[col2] = 1;
                }
                anchorCols[col2]++;
                critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
            } else if (row1 > row2) { //going up
                startPty = startPty - anchorGap;
                endPty = endPty + (anchorGap * 2);
                row1 = row1 - 1;
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = (row1) * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                    anchorCols[col2] = 1;
                }
                anchorCols[col2]++;
                critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
            } else { //same row
                startPty = startPty + anchorGap;
                endPty = endPty + (anchorGap * 2);
                if (anchorCols[col1] == undefined || anchorCols[col1] >= numAnchors) {
                    anchorCols[col1] = 1;
                }
                if (anchorRows[row1] == undefined || anchorRows[row1] >= numAnchors) {
                    anchorRows[row1] = 1;
                }
                anchorCols[col1]++;
                anchorRows[row1]++;
                critPt1x = startPtx + (anchorCols[col1] * anchorGap);
                critPt1y = row1 * (boxheight + boxgap) + topgap + boxheight + (anchorRows[row1] * anchorGap);
                if (anchorCols[col2] == undefined || anchorCols[col2] >= numAnchors) {
                    anchorCols[col2] = 1;
                }
                anchorCols[col2]++;
                critPt2x = endPtx - boxgap + (anchorCols[col2] * anchorGap);
            }
            ctx.moveTo(startPtx, startPty);
            ctx.lineTo(critPt1x, startPty);
            ctx.lineTo(critPt1x, critPt1y);
            ctx.lineTo(critPt2x, critPt1y);
            ctx.lineTo(critPt2x, endPty);
            ctx.lineTo(endPtx, endPty);
            ctx.stroke();
            ctx.strokeStyle = "#000000";
        }
    }

    var startingSemester = $('#startingSemester').data("startingsemester");
    var planid = $('#startingSemester').data("planid");

    var fullRedraw = function () {
        courseList = [];
        $.ajax('/Plans/GetPlanCourses/' + planid, {
            'dataType': 'json',
            'success': function (data, status, jqXhr) {
                if (clickedIndex == -1) {
                    var i = 0;
                    $(data).each(function (idx, obj) {
                        //$('#courses').append('<li>' + obj.courseTitle + '</li>');
                        var course = new Course(obj.pcourseID, obj.courseID, obj.courseTitle, obj.courseName, obj.elistID, obj.elistName, obj.semester, obj.order, obj.hours, obj.prereq);
                        courseList.push(course);
                        if (semesterList[course.semester - semesterOffset].show == 'false') {
                            addSemester(course.semester);
                        }
                        //drawCourse(course);
                        i++;
                    })
                    clear();
                    redraw();
                }
            }
        });
        setTimeout(fullRedraw, 30000);
    };


    $.ajax('/Plans/GetSemesters/' + startingSemester, {
        'dataType': 'json',
        'success': function (data, status, jqXhr) {
            var i = 0;
            $(data).each(function(idx, obj) {
                //$('#semesters').append('<li>' + obj.semesterName + '</li>');
                var semester = new Semester(obj.ID, obj.semesterName, 'false', -1);
                semesterList.push(semester);
                //drawSemester(semester);
                i++;
            })
            semesterOffset = semesterList[0].semesterID;
            //clear();
            //redraw();
            fullRedraw();
        }
    });
 

    $('#flowchartCanvas').bind('mousedown', function (event) {
        var x = event.pageX - this.offsetLeft;
        var y = event.pageY - this.offsetTop;

        courseList.forEach(function (course, index) {
            if (course.x <= x && x <= course.x + boxwidth && course.y <= y && y <= course.y + boxheight) {
                clickedIndex = index;
            }
        });
        if (clickedIndex > -1) {
            $('#flowchartCanvas').bind('mousemove', function (event) {
                var x = event.pageX - this.offsetLeft;
                var y = event.pageY - this.offsetTop;
                clear();
                drawDropZone(x, y);
                redraw();
                drawCourseDrag(courseList[clickedIndex], x, y);
            });
        } else {
            $('#flowchartCanvas').unbind('mousemove');
        }
    });

    $('#flowchartCanvas').bind('mouseup', function (event) {
        $('#flowchartCanvas').unbind('mousemove');
        if (clickedIndex > -1) {
            var x = event.pageX - this.offsetLeft;
            var y = event.pageY - this.offsetTop;
            dropCourse(x, y);
            courseList[clickedIndex].clicked = false;
        }
        clickedIndex = -1;
        clear();
        redraw();
    });

    // onHide : fade the window out, remove overlay after fade.
    var myClose = function (hash) {
        hash.w.fadeOut('2000', function () { hash.o.remove(); });
        courseList = Array();
        $.ajax('/Plans/GetPlanCourses/' + planid, {
            'dataType': 'json',
            'success': function (data, status, jqXhr) {
                var i = 0;
                $(data).each(function (idx, obj) {
                    //$('#courses').append('<li>' + obj.courseTitle + '</li>');
                    var course = new Course(obj.pcourseID, obj.courseID, obj.courseTitle, obj.courseName, obj.elistID, obj.elistName, obj.semester, obj.order, obj.hours, obj.prereq);
                    courseList.push(course);
                    if (semesterList[course.semester - semesterOffset].show == 'false') {
                        addSemester(course.semester);
                    }
                    //drawCourse(course);
                    i++;
                })
                clear();
                redraw();
            }
        });
    };

    $('#flowchartCanvas').on('dblclick', function (event){
        var x = event.pageX - this.offsetLeft;
        var y = event.pageY - this.offsetTop;

        courseList.forEach(function (course, index) {
            if (course.x <= x && x <= course.x + boxwidth && course.y <= y && y <= course.y + boxheight) {
                clickedIndex = index;
            }
        });
        if (clickedIndex > -1) {
            var course = courseList[clickedIndex];
            $('#popup').jqm({ ajax: '/Plans/UpdateCourseInfo/' + course.pcourseID, onHide : myClose}).jqmShow();
        }
    });

    $("#addSemester").on('click', function (event) {
        event.preventDefault();
        addSemester($("#semesterID").val());
        clear();
        redraw();
    });

});