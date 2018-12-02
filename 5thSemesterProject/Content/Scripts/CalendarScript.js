

console.log("Ready!");

var showingDate = 0.0;

var showingDateBtn = document.getElementById("showingDay");
var date = new Date();

// To populate the button with current date
today();

function prevDay() {
	showingDate--;
	showingDateBtn.innerHTML = addDays(date, showingDate).getDate() + "-" + (addDays(date, showingDate).getMonth() + 1) + "-" + addDays(date, showingDate).getFullYear(); // add 1 because getMonth() starts at 0
}

function today() {
	showingDate = 0.0;
	showingDateBtn.innerHTML = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
}

function nextDay() {
	showingDate++;
	showingDateBtn.innerHTML = addDays(date, showingDate).getDate() + "-" + (addDays(date, showingDate).getMonth() + 1) + "-" + addDays(date, showingDate).getFullYear(); // add 1 because getMonth() starts at 0
}

function addDays(date, days) {
	var result = new Date(date);
	result.setDate(result.getDate() + days);
	return result;
}