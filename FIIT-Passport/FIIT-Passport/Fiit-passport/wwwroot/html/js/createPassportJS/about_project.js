async function getAdoutPassport() {
    await getPassport().then(data => {
        document.querySelector('.name_customer_input').value = data.ordererName;
        document.querySelector('.name_project_input').value = data.projectName;
        document.querySelector('.description_project_input').value = data.projectDescription;
    })
}

async function Update() {
    await updatePassport({ SessionId: getCookie("idSession"),
        OrdererName: document.querySelector('.name_customer_input').value, 
        ProjectName: document.querySelector('.name_project_input').value,   
        ProjectDescription: document.querySelector('.description_project_input').value
    });
}

function saveFields() {
    const object = {
        name_customer_input : document.querySelector('.name_customer_input').value,
        name_project_input : document.querySelector('.name_project_input').value,
        description_project_input : document.querySelector('.description_project_input').value
    };
    localStorage.setItem('about_project', JSON.stringify(object));
}

function uploadFields() {
    const object = JSON.parse(localStorage.getItem('about_project'));
    if (object === null){
        return;
    }
    document.querySelector('.name_customer_input').value = object.name_customer_input || '';
    document.querySelector('.name_project_input').value = object.name_project_input || '';
    document.querySelector('.description_project_input').value = object.description_project_input || '';
}

window.onload = function() {
    uploadFields();
    resizeTextares();
    document.body.addEventListener('input', function(event) {
        if (event.target.tagName === 'TEXTAREA') {
            autoResizeTextarea(event.target);
        }
    });
    
    window.addEventListener('resize', function(event) {
        resizeTextares(event.target);
    });
};

window.onbeforeunload = function() {
    saveFields();
};