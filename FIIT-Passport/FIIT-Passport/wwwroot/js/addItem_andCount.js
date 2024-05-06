document.addEventListener('DOMContentLoaded', function() {
    const sendList = document.querySelector('.send_list');
    const checkingList = document.querySelector('.checking_list');
    const checkedList = document.querySelector('.checked_list');
    const sendCount = document.querySelector('.send_count');
    const checkingCount = document.querySelector('.checking_count');
    const checkedCount = document.querySelector('.checked_count');

    function addItemToList(list, itemText) {
        const newItem = document.createElement('li');
        newItem.textContent = itemText;
        newItem.classList.add('request');
        newItem.draggable = 'true';
        list.appendChild(newItem);
    }

    function countItemsAndUpdateCount(list, countElement) {
        countElement.textContent = list.children.length;
    }

    addItemToList(checkingList, 'Какое-то название проекта');
    addItemToList(checkedList, 'Какое-то название проекта');

    countItemsAndUpdateCount(sendList, sendCount);
    countItemsAndUpdateCount(checkingList, checkingCount);
    countItemsAndUpdateCount(checkedList, checkedCount);
});
