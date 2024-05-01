var inputs = document.querySelectorAll("input");
inputs.forEach(function(input) {
    input.addEventListener("blur", function() {
        input.checkValidity();
        }
    );
});
