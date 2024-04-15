document.addEventListener("DOMContentLoaded", function() {
    const prevButton = document.querySelector(".ex_left_block");
    const nextButton = document.querySelector(".ex_right_block");
    const blocks = document.querySelectorAll(".example_block");
    let currentIndex = 0;

    function showBlock(index) {
        blocks.forEach((block, i) => {
            if (i === index) {
                block.style.display = "flex";
            } else {
                block.style.display = "none";
            }
        });
    }

    function goToPrev() {
        currentIndex = (currentIndex - 1 + blocks.length) % blocks.length;
        showBlock(currentIndex);
    }

    function goToNext() {
        currentIndex = (currentIndex + 1) % blocks.length;
        showBlock(currentIndex);
    }

    prevButton.addEventListener("click", function(event) {
        event.preventDefault();
        goToPrev();
    });

    nextButton.addEventListener("click", function(event) {
        event.preventDefault();
        goToNext();
    });

    showBlock(currentIndex);
});
