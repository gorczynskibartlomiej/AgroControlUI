document.addEventListener("DOMContentLoaded", function () {
    var descriptionModalBody = document.getElementById("descriptionModalBody");

    document.querySelectorAll("[data-bs-target='#descriptionModal']").forEach(function (button) {
        button.addEventListener("click", function () {
            var description = this.getAttribute("data-description") || "Brak opisu";
            descriptionModalBody.textContent = description;
        });
    });
});
