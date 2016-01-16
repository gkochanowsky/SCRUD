/*
	Created By Gene Kochanowsky	

	All I ask is that you say who you stole this from.
*/
function Toggle(toggleID, rowID, action, funcFinish, open, colspan) {
	var a = $("#" + toggleID);
	if (a.length == 0) return false;
	var state = "open";
	if (a.attr("class").indexOf("glyphicon-chevron-right") != -1 || Boolean(open)) state = "closed";
	switch (state) {
		case "closed":
			LoadToggle(toggleID, rowID, action, funcFinish, colspan);
			break;
		case "open":
			ToggleClose(toggleID, rowID);
			break;
	}
	return false;
}

function LoadToggle(toggleID, rowID, action, funcFinish, colspan) {
	$.ajaxSetup({ cache: false, });
	$.get(action, null, function (html) {
		var row = $("#" + rowID);
		var cspan = row.find('td').length;
		if (Boolean(colspan)) cspan = colspan;
		var kidsID = rowID + "-children";
		var kidsRow = $("#" + kidsID);
		var rowHtml = "";
		if (kidsRow.length > 0) {
			rowHtml = "<td colspan='" + cspan + "'>" + html + "</td>";
			kidsRow.html(rowHtml);
		} else {
			rowHtml = "<tr id='" + kidsID + "'><td colspan='" + cspan + "'>" + html + "</td></tr>";
			row.after(rowHtml);
		}
		var a = $("#" + toggleID);
		SetToggle(toggleID);
		if (Boolean(funcFinish))
			funcFinish();
		return kidsID;
	}).fail(function (jqXHR, textStatus, errorThrown) {
		OnFailure(jqXHR, textStatus, errorThrown);
	});
}

function ToggleClose(toggleID, rowID) {
	var a = $("#" + toggleID);
	var kidsID = rowID + "-children";
	$("#" + kidsID).remove();;
	SetToggle(toggleID);
	return false;
}

function SetToggle(toggleID) {
	var a = $("#" + toggleID);
	var cls = a.attr("class");
	if (cls.indexOf("glyphicon-chevron-right") == -1)
		cls = cls.replace("glyphicon-chevron-down", "glyphicon-chevron-right");
	else
		cls = cls.replace("glyphicon-chevron-right", "glyphicon-chevron-down")
	a.attr("class", cls);
}