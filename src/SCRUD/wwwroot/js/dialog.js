/*
	Created By Gene Kochanowsky	

	All I ask is that you say who you stole this from.
*/
function OpenDialog(viewID, url, hookupEvents) {
	if ($("#" + viewID).length == 0) GenerateView(viewID);
	var view = $("#" + viewID);
	$.ajaxSetup({ cache: false, error: OnFailure });
	$.get(url, null, function (result) {
		if (!Boolean(result)) return;
		view.html(result);
		FixValidation(view);
		if (Boolean(hookupEvents)) hookupEvents();
		view.modal("show");
		return view;
	});
}

function PostDialog(viewID, formID, preFunc, failFunc, successFunc, skipVal) {
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
			if (Boolean(successFunc)) successFunc();
		}
		else {
			view.html(result);
		}
	}).fail(function (jqXHR, textStatus, errorThrown) {
		OnFailure(jqXHR, textStatus, errorThrown);
		if (Boolean(failFunc)) failFunc();
	});
}

function CloseDialog(viewID) {
	var v = $("#" + viewID);
	if (v.length == 0) return;
	v.modal("hide");
	$('body').removeClass('modal-open');
	$('.modal-backdrop').remove();
	v.html("");
	return false;
}

function GenerateView(view) {
	var dlg = '<div id="' + view + '" role="dialog" class="modal fade">';
	document.body.insertAdjacentHTML('beforeend', dlg)
	$("#" + view).modal("hide");
}

function FixValidation(view) {
	var form = view.find('form');
	$.each(form, function (i, frm) {
		$(this).removeData('validator');
		$(this).removeData('unobtrusiveValidation');
		var formID = $(this).attr('id');
		$.validator.unobtrusive.parse('#' + formID);
	});
}

function OnFailure(xhr, status, error) {
	if (confirm("View failure " + error + " in a new browser window?")) {
		var win = window.open();
		win.document.write(xhr.responseText);
	}
}