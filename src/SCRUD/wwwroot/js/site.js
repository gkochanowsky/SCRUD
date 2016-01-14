// Write your Javascript code.
$(function () {
	hookupDTP();
});

function hookupDTP() {
	$(".datetimepicker").datetimepicker();
	$(".datepicker").datetimepicker({
		format: "MM/DD/YYYY"
	});
}
