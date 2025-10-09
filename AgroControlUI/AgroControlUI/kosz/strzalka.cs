//using Microsoft.CodeAnalysis.Scripting;
//using NetTopologySuite.Index.HPRtree;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Collections.Generic;

//namespace AgroControlUI.kosz
//{
//    public class strzalka
//    {
//        @model List<AgroControlUI.DTOs.Fertilizers.FertilizerCategoryDto>

//@await Html.PartialAsync("_DeleteConfirmationModal")

//<link rel = "stylesheet" href="~/css/indexList.css">
//<link rel = "stylesheet" href="~/css/listOperations.css">

//<h1 class="green">Kategorie nawozów</h1>

//<div id = "categories" class="wrapper">
//    <p>
//        <a asp-controller="FertilizerCategory" asp-action="Create" class="add-btn btn"><img src = "~/images/plus-solid.svg" /> Dodaj kategorię nawozów</a>
//    </p>
//    <div class="table">
//        <div class="row header green" id="headerRow">
//            <div class="cell" data-sort="name" onclick="sortByColumn(this)">
//                Nazwa kategorii<span class="sort-arrow">▼</span>
//            </div>
//            <div class="cell">
//                Akcje
//            </div>
//        </div>
//        <div class="row">
//            <div class="cell" id="searchWrapper">
//                <input type = "text" id="searchName" class="search-input" placeholder="Szukaj..." oninput="searchByName()" />
//            </div>
//            <div class="cell">
//            </div>
//        </div>
//        <div class="list">
//        @foreach(var category in Model)
//        {
//            < div class="row">
//                <div class="cell name" data-title="Nazwa kategorii">
//                    @category.Name
//                </div>
//                <div class="cell" data-title="Akcje">
//                    <div class="action-buttons">
//                        <a asp-action="Edit" asp-route-id="@category.Id" class="edit-btn"><img src = "~/images/edit.svg" /> Edytuj </ a >
//                        < button type="button" class="delete-btn" data-bs-toggle="modal" data-bs-target="#deleteModal" onclick="setDeleteUrl('@Url.Action("DeleteConfirmed", new { id = category.Id })')">
//                            <img src = "~/images/delete.svg" /> Usuń
//                        </ button >
//                    </ div >
//                </ div >
//            </ div >
//        }
//    </div>
//    </div>
//    <ul class="pagination"></ul>
//</div>
//<script src = "https://cdnjs.cloudflare.com/ajax/libs/list.js/2.3.1/list.min.js" ></ script >
//< script src="~/js/listOperations.js"></script>
//<script src = "~/js/deleteModal.js" ></ script >

////search
//document.addEventListener('DOMContentLoaded', function() {
//    myList.sort('name', { order: 'asc' });
//});

//function searchByName()
//{
//    var searchValue = document.getElementById("searchName").value.toLowerCase();
//    myList.filter(function(item) {
//        var categoryName = item.values().name.toLowerCase();
//        return categoryName.includes(searchValue);
//    });
//}
////sort
//var headerCells = document.querySelectorAll("#headerRow .cell[data-sort]");
//var dynamicValueNames = [];
//headerCells.forEach(function(cell) {
//    var sortKey = cell.getAttribute("data-sort");
//    if (sortKey && dynamicValueNames.indexOf(sortKey) === -1)
//    {
//        dynamicValueNames.push(sortKey);
//    }
//});

//function sortByColumn(headerCell)
//{
//    var column = headerCell.getAttribute("data-sort"); // Pobieramy nazwę kolumny do sortowania
//    if (!column) return;

//    // Resetujemy strzałki dla wszystkich kolumn
//    document.querySelectorAll(".sort-arrow").forEach(arrow => arrow.style.display = "none");

//    // Ustawiamy domyślny kierunek sortowania, jeśli jeszcze nie było sortowane
//    if (!sortOrder[column])
//    {
//        sortOrder[column] = "desc";
//    }

//    // Sortujemy tabelę
//    myList.sort(column, { order: sortOrder[column] });

//    // Pobieramy strzałkę w klikniętej kolumnie i ustawiamy jej widoczność
//    var sortArrow = headerCell.querySelector(".sort-arrow");
//    if (sortArrow)
//    {
//        sortArrow.style.display = "inline-block";

//        // Odwracamy kierunek sortowania na kolejny klik
//        if (sortOrder[column] === "asc")
//        {
//            sortOrder[column] = "desc";
//            sortArrow.style.transform = "rotate(0deg)";
//        }
//        else
//        {
//            sortOrder[column] = "asc";
//            sortArrow.style.transform = "rotate(180deg)";
//        }
//    }
//}

//// Ukrywamy wszystkie strzałki na starcie
//window.onload = function() {
//    document.querySelectorAll(".sort-arrow").forEach(arrow => arrow.style.display = "none");
//};
////var sortOrder = "desc";
////function sortByName() {
////    myList.sort('name', { order: sortOrder });

////    var sortArrow = document.getElementById("sortArrow");

////    if (sortOrder === "asc") {
////        sortOrder = "desc";
////        sortArrow.style.transform = "rotate(0deg)";
////    } else {
////        sortOrder = "asc";
////        sortArrow.style.transform = "rotate(180deg)";
////    }
////}
////pagination
//var options = {
//    valueNames: dynamicValueNames,
//    ListClass:['list'],
//    page: 2,
//    pagination:
//[{
//paginationClass: "pagination",
//        innerWindow: 1,
//        outerWindow: 1
//    }]
//};
//var myList = new List('categories', options);
//a teraz?
//    }
//}
