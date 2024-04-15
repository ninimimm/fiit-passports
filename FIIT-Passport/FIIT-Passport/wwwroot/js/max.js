document.addEventListener('DOMContentLoaded', function() {
    const numberInput = document.getElementById('max_input');
    const increaseButton = document.getElementById('max_plus');
    const decreaseButton = document.getElementById('max_minus');

    increaseButton.addEventListener('click', function() {
        numberInput.stepUp();
    });

    decreaseButton.addEventListener('click', function() {
        numberInput.stepDown();
    });
});
