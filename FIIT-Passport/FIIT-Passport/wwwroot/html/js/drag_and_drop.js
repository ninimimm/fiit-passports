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


const columns = document.querySelectorAll('.column');
let lastAdded = null;

function createEmptyBlock(block) {
    const emptyBlock = document.createElement('li');
    emptyBlock.classList.add('request');
    emptyBlock.innerHTML = block.innerHTML;
    emptyBlock.id = 'empty';
    const opacity = '0.5';
    emptyBlock.style.color = `rgba(0, 0, 0, ${opacity})`;
    emptyBlock.style.backgroundColor = `rgba(255, 255, 255, ${opacity})`;
    emptyBlock.style.border = '1.2px dashed black';
    emptyBlock.querySelectorAll('*').forEach(element => {
        element.style.opacity = opacity;
    });
    return emptyBlock;
}


const shifts = {
    shiftX: 0,
    shiftY: 0,
    set: (event) => {
        shifts.shiftX = event.clientX - event.target.getBoundingClientRect().left;
        shifts.shiftY = event.clientY - event.target.getBoundingClientRect().top;
    },
};

const initialMovingElementPageXY = {
    x: 0,
    y: 0,
    set: (event) => {
        const rect = event.target.getBoundingClientRect();
        initialMovingElementPageXY.x = rect.x + window.scrollX;
        initialMovingElementPageXY.y = rect.y + window.scrollY;
    },
};

function insertAfter(referenceNode) {
    if (referenceNode.nextSibling) {
        if (referenceNode.nextSibling.id != 'empty') {
            lastAdded.parentNode.removeChild(lastAdded);
            referenceNode.parentNode.insertBefore(lastAdded, referenceNode.nextSibling);
        }
        return;
    }
    lastAdded.parentNode.removeChild(lastAdded);
    referenceNode.parentNode.appendChild(lastAdded);
}

function insertBefore(referenceNode) {
    if (referenceNode.previousSibling) {
        if (referenceNode.previousSibling.id != 'empty') {
            lastAdded.parentNode.removeChild(lastAdded);
            referenceNode.parentNode.insertBefore(lastAdded, referenceNode);
        }
        return;
    }
    lastAdded.parentNode.removeChild(lastAdded);
    referenceNode.parentNode.insertBefore(lastAdded, referenceNode);
}

const getElementCoordinates = (node, searchCoordsBy) => {
    const rect = node.getBoundingClientRect();
    return {
      top:
        searchCoordsBy == "by-center"
          ? rect.top + rect.height / 2
          : rect.top + 10,
      left: rect.left + rect.width / 2,
    };
  };

const getElementBelow = (movingElement, searchCoordsBy) => {
    const movingElementCenter = getElementCoordinates(
      movingElement,
      searchCoordsBy
    );
    movingElement.hidden = true;
    let elementBelow = document.elementFromPoint(
      movingElementCenter.left,
      movingElementCenter.top
    );
    movingElement.hidden = false;
    return elementBelow;
  };

function onMouseDown(event) {
    event.preventDefault();
    shifts.set(event);
    lastAdded = createEmptyBlock(event.target);
    event.target.parentNode.insertBefore(lastAdded, event.target);
    event.target.style.position = 'absolute';
    event.target.style.zIndex = '1000';
    initialMovingElementPageXY.set(event);
    event.target.style.transform = `translate(${
        event.pageX - initialMovingElementPageXY.x - shifts.shiftX
      }px, ${
        event.pageY - initialMovingElementPageXY.y - shifts.shiftY
      }px)`;
    event.target.addEventListener('mousemove', onMouseMove);
    event.target.addEventListener('mouseup', onMouseUp);
}

function onMouseMove(event) {
    event.preventDefault();
    event.target.style.transform = `translate(${
        event.pageX - initialMovingElementPageXY.x - shifts.shiftX
      }px, ${
        event.pageY - initialMovingElementPageXY.y - shifts.shiftY
      }px)`;
    let elementBelow = getElementBelow(event.target, "by-center");
    if (elementBelow.id == 'empty') {
        return;
    }
    let elRect = elementBelow.getBoundingClientRect();
    if (elementBelow.classList.contains('request')) {
        if (event.clientY < elRect.top + elRect.height / 2) {
            insertBefore(elementBelow);
        } else {
            insertAfter(elementBelow);
        }
    } else {
        if (elementBelow.classList.contains('list')) {
            if (elementBelow.children.length == 0 || (event.clientY > elementBelow.children[elementBelow.children.length - 1].getBoundingClientRect().top && elementBelow.children[elementBelow.children.length - 1].id != 'empty')) {
                lastAdded.parentNode.removeChild(lastAdded);
                elementBelow.appendChild(lastAdded);
            } else if (event.clientY < elementBelow.children[0].getBoundingClientRect().top && elementBelow.children[0].id != 'empty') {
                lastAdded.parentNode.removeChild(lastAdded);
                elementBelow.insertBefore(lastAdded, elementBelow.firstChild);
            }
        }
    }
};

function onMouseUp(event) {
    event.preventDefault();
    event.target.removeEventListener('mousemove', onMouseMove);
    event.target.removeEventListener('mouseup', onMouseUp);
    Object.assign(event.target.style, {
        position: 'static',
        left: "auto",
        top: "auto",
        zIndex: "auto",
        transform: "none",
      });
    var countHome = event.target.parentNode.parentNode.querySelector('span:last-of-type');
    var countNext = lastAdded.parentNode.parentNode.querySelector('span:last-of-type');
    countHome.textContent = +(countHome.textContent) - 1;
    countNext.textContent = +(countNext.textContent) + 1;
    lastAdded.parentNode.insertBefore(event.target, lastAdded);
    lastAdded.parentNode.removeChild(lastAdded);
}

document.querySelectorAll('.request').forEach(element => {
    element.addEventListener('mousedown', onMouseDown);
});

console.log(document.querySelectorAll('.request'));
