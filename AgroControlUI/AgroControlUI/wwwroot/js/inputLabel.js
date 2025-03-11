document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".group input, .group textarea").forEach(function (field) {
        function checkFilled() {
            if (field.type === "number") {
                field.classList.toggle("filled", field.value.trim() !== "");
            } else {
                field.classList.toggle("filled", field.value.trim() !== "");
            }
        }

        // Sprawdza, czy input ma już wartość na starcie (np. podczas edycji)
        checkFilled();

        // Nasłuchuje zmiany wprowadzane przez użytkownika
        field.addEventListener("input", checkFilled);
        field.addEventListener("blur", function () {
            // Upewniamy się, że klasa 'filled' jest przypisana, nawet jeśli pole ma błąd walidacji
            checkFilled();
        });
    });

    document.querySelectorAll("input[type='hidden']").forEach(function (field) {
        field.classList.remove("filled");
    });
    document.querySelectorAll(".group input[type='number']").forEach(function (field) {
        function checkFilled() {
            if (field.type === "number") {
                field.classList.toggle("filled", field.value.trim() !== "");
                console.log("Dopisalem do numbera filled")
            } else {
                field.classList.toggle("filled", field.value.trim() !== "");
            }
        }
        field.addEventListener("blur", function () {
            // Sprawdzamy, czy pole zawiera błąd walidacji (klasa input-validation-error)
            const validationError = field.closest('.group').querySelector('.input-validation-error');
            if (validationError) {
                // Jeśli pole ma błąd walidacji, nie usuwamy klasy 'filled'
                field.classList.add("filled");
            } else {
                // Jeśli nie ma błędu walidacji, sprawdzamy, czy pole jest wypełnione
                checkFilled(field);
            }
        });
    });
});

//function validateInput(event) {
//    var input = event.target;
//    var value = input.value;

//    // Zamiana przecinka na kropkę
//    value = value.replace(',', '.');

//    // Ustawienie poprawionej wartości w polu
//    input.value = value;
//}
