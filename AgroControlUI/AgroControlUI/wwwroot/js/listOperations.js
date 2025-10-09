
//document.addEventListener('DOMContentLoaded', function () {
//    var firstSortableColumn = document.querySelector("#headerRow .cell[data-sort]");

//    if (firstSortableColumn) {
//        sortByColumn(firstSortableColumn);
//    }

//    var sortArrow = firstSortableColumn.querySelector(".sort-arrow");
//    if (sortArrow) {
//        sortArrow.style.display = "inline-block";

//    }
//});

//function searchByColumn() {
//    var filters = {};

//    document.querySelectorAll(".search-input").forEach(function (input) {
//        var columnKey = input.getAttribute("data-search");
//        var value = input.value.toLowerCase().trim();

//        if (value !== "") {
//            filters[columnKey] = value;
//        }
//    });

//    myList.filter(function (item) {
//        return Object.keys(filters).every(function (key) {
//            var itemValue = item.values()[key].toLowerCase();
//            return itemValue.includes(filters[key]);
//        });
//    });

//}
//var headerCells = document.querySelectorAll("#headerRow .cell[data-sort]");
//var dynamicValueNames = [];
//headerCells.forEach(function (cell) {
//    var sortKey = cell.getAttribute("data-sort");
//    if (sortKey && dynamicValueNames.indexOf(sortKey) === -1) {
//        dynamicValueNames.push(sortKey);
//    }
//});
//var sortOrder = {};
//function sortByColumn(headerCell) {
//    var column = headerCell.getAttribute("data-sort");
//    if (!column) return;

//    document.querySelectorAll(".sort-wrapper").forEach(function (wrapper) {
//        var arrow = wrapper.querySelector(".sort-arrow");
//        if (arrow) {
//            arrow.style.visibility = "hidden";
//            arrow.classList.remove("asc", "desc");
//            wrapper.classList.remove("sort-arrow-visible");
//        }
//    });

//    if (!sortOrder[column]) {
//        sortOrder[column] = "asc";
//    }

//    myList.sort(column, { order: sortOrder[column] });

//    var wrapper = headerCell.querySelector(".sort-wrapper");
//    var sortArrow = wrapper ? wrapper.querySelector(".sort-arrow") : null;
//    if (sortArrow) {
//        wrapper.classList.add("sort-arrow-visible");

//        if (sortOrder[column] === "asc") {
//            sortOrder[column] = "desc";
//            sortArrow.classList.remove("asc");
//            sortArrow.classList.add("desc");
//        } else {
//            sortOrder[column] = "asc";
//            sortArrow.classList.remove("desc");
//            sortArrow.classList.add("asc");
//        }
//    }
//}
//var options = {
//    valueNames: dynamicValueNames,
//    ListClass:'list',
//    page: 10,
//    pagination: [{
//        paginationClass: "pagination",
//        innerWindow: 1,
//        outerWindow: 1
//    }]
//};
//var myList = new List('categories', options);

document.addEventListener('DOMContentLoaded', function () {
    var firstSortableColumn = document.querySelector("#headerRow .cell[data-sort]");

    if (firstSortableColumn) {
        sortByColumn(firstSortableColumn);
    }

    var sortArrow = firstSortableColumn.querySelector(".sort-arrow");
    if (sortArrow) {
        sortArrow.style.display = "inline-block";
    }
});

function searchByColumn() {
    var filters = {};

    document.querySelectorAll(".search-input").forEach(function (input) {
        var columnKey = input.getAttribute("data-search");
        var value = input.value.toLowerCase().trim();

        if (value !== "") {
            filters[columnKey] = value;
        }
    });

    myList.filter(function (item) {
        return Object.keys(filters).every(function (key) {
            var itemValue = item.values()[key].toLowerCase();
            return itemValue.includes(filters[key]);
        });
    });

    updateNoResultsVisibility(); 
}

var headerCells = document.querySelectorAll("#headerRow .cell[data-sort]");
var dynamicValueNames = [];
headerCells.forEach(function (cell) {
    var sortKey = cell.getAttribute("data-sort");
    if (sortKey && dynamicValueNames.indexOf(sortKey) === -1) {
        dynamicValueNames.push(sortKey);
    }
});

var sortOrder = {};
function sortByColumn(headerCell) {
    var column = headerCell.getAttribute("data-sort");
    if (!column) return;

    document.querySelectorAll(".sort-wrapper").forEach(function (wrapper) {
        var arrow = wrapper.querySelector(".sort-arrow");
        if (arrow) {
            arrow.style.visibility = "hidden";
            arrow.classList.remove("asc", "desc");
            wrapper.classList.remove("sort-arrow-visible");
        }
    });

    if (!sortOrder[column]) {
        sortOrder[column] = "asc";
    }

    myList.sort(column, { order: sortOrder[column] });

    var wrapper = headerCell.querySelector(".sort-wrapper");
    var sortArrow = wrapper ? wrapper.querySelector(".sort-arrow") : null;
    if (sortArrow) {
        wrapper.classList.add("sort-arrow-visible");

        if (sortOrder[column] === "asc") {
            sortOrder[column] = "desc";
            sortArrow.classList.remove("asc");
            sortArrow.classList.add("desc");
        } else {
            sortOrder[column] = "asc";
            sortArrow.classList.remove("desc");
            sortArrow.classList.add("asc");
        }
    }
}

var options = {
    valueNames: dynamicValueNames,
    ListClass: 'list',
    page: 10,
    pagination: [{
        paginationClass: "pagination",
        innerWindow: 1,
        outerWindow: 1
    }]
};

var myList = new List('categories', options);

function updateNoResultsVisibility() {
    var noResultsMessage = document.getElementById("no-results-message");
    var visibleItems = myList.visibleItems;

    if (visibleItems.length === 0) {
        console.log('Brak wyników.');

        if (!noResultsMessage) {
            noResultsMessage = document.createElement('div');
            noResultsMessage.id = 'no-results-message';
            noResultsMessage.innerText = 'Brak wyników';
            document.querySelector(".pagination").appendChild(noResultsMessage);
        }
    } else {
        console.log('Są wyniki.');

        if (noResultsMessage) {
            noResultsMessage.remove();
        }
    }
}

myList.on('updated', function () {
    updateNoResultsVisibility();
});
