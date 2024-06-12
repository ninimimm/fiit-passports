async function getOthersPassport() {
    await getPassport().then(data => {
        document.querySelector('.max_input').value = data.copiesNumber;
        document.querySelector('.address_input').value = data.meetingLocation;
    })
}

async function Update() {
    await updatePassport({ SessionId: getCookie("idSession"),
        CopiesNumber: document.querySelector('.max_input').value, 
        MeetingLocation: document.querySelector('.address_input').value,   
    });
}  

function saveFields() {
    const object = {
        max_input : document.querySelector('.max_input').value,
        address_input : document.querySelector('.address_input').value
    };
    localStorage.setItem('others_project', JSON.stringify(object));
}

function uploadFields() {
    const object = JSON.parse(localStorage.getItem('others_project'));
    if (object === null){
        return;
    }
    document.querySelector('.max_input').value = object.max_input || '';
    document.querySelector('.address_input').value = object.address_input || '';
}

window.onload = function() {
    uploadFields();
};

window.onbeforeunload = function() {
    saveFields();
};