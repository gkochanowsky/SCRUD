/*
	Created By Gene Kochanowsky	
*/
function OpenDialog(viewID, url) {
	if ($("#" + viewID).length == 0) GenerateView(viewID);
	var view = $("#" + viewID);
	$.ajaxSetup({ cache: false, error: OnFailure });
	$.get(url, null, function (result) {
		if (!Boolean(result)) return;
		view.html(result);
		view.modal("show");
		return view;
	});
}

function PostDialog(viewID, formID, postFunc, preFunc, skipVal) {
	var view = $("#" + viewID);
	var form = $("#" + formID);
	if (view.length == 0 || form.length == 0) return false;
	if (!Boolean(skipVal)) if (!form.valid()) return false;
	$.ajaxSetup({ cache: false });
	var data = form.serialize();
	var url = form.attr("action");
	if (Boolean(preFunc)) preFunc();
	$.post(url, data, function (result) {
		var isValid = true;
		if (result == "SUCCESS" || result == "") {
			CloseDialog(viewID);
		}
		else {
			view.html(result);
		}
	});
}

function CloseDialog(viewID) {
	var v = $("#" + viewID);
	if (v.length == 0) return;
	v.modal("hide");
	v.html("");
	return false;
}

function GenerateView(view) {
	var dlg = '<div id="' + view + '" role="dialog" class="modal fade">';
	document.body.insertAdjacentHTML('beforeend', dlg)
	$("#" + view).modal("hide");
}

function OnFailure(xhr, status, error) {
	if (confirm("View failure " + error + " in a new browser window?")) {
		var win = window.open();
		win.document.write(xhr.responseText);
	}
}