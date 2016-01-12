/*
	Created By Gene Kochanowsky	
*/
function LoadSearchResults(page, formID, entireView, afterFunc) {
	var form = $("#" + formID);
	if (form.length == 0) return false;
	var pg = form.find("#Page");
	var npg = Boolean(page) ? page : 1
	form.find("#Page").val(npg);
	$.ajaxSetup({ cache: false, error: SearchFailure });
	var data = form.serialize();
	var url = form.attr("action");
	$.post(url, data, function (result) {
		var viewNameVar = "#ResultView";
		if (Boolean(entireView))
			viewNameVar = "#FormView";
		var viewName = form.find(viewNameVar).val();
		var view = $("#" + viewName);
		if (view.length == 0) return false;
		view.html(result);
		if (Boolean(afterFunc))
			afterFunc();
	});
}

function SortSearchResults(col, formID) {
	var form = $("#" + formID);
	if (form == null) return false;
	var sorthv = form.find("#sort");
	var dirhv = form.find("#sortDir");
	var pagehv = form.find("#Page");
	if (sorthv == null || dirhv == null || pagehv == null) return false;
	var lastSort = sorthv.val();
	var lastDir = dirhv.val();
	if (col == lastSort) {
		dirhv.val(lastDir == "ASC" ? "DESC" : "ASC");
	}
	else {
		sorthv.val(col);
		dirhv.val("ASC");
	}
	LoadSearchResults(pagehv.val(), formID);
}

function SearchFailure(xhr, status, error) {
	if (confirm("View failure " + error + " in a new browser window?")) {
		var win = window.open();
		win.document.write(xhr.responseText);
	}
}