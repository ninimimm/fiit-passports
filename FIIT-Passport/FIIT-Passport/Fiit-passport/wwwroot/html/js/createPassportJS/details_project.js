async function getDetailsPassport() {
    await getPassport().then(data => {
        document.querySelector('.goal_project_input').value = data.goal;
        document.querySelector('.result_project_input').value = data.result;
        document.querySelector('.criteria_project_input').value = data.acceptanceCriteria;
    });
}

async function Update() {
    await updatePassport({ SessionId: getCookie("idSession"),
        Goal: document.querySelector('.goal_project_input').value, 
        Result: document.querySelector('.result_project_input').value,   
        AcceptanceCriteria: document.querySelector('.criteria_project_input').value
    });
}   

function saveFields() {
    const object = {
        goal_project_input : document.querySelector('.goal_project_input').value,
        result_project_input : document.querySelector('.result_project_input').value,
        criteria_project_input : document.querySelector('.criteria_project_input').value
    };
    localStorage.setItem('details_project', JSON.stringify(object));
}

function uploadFields() {
    const object = JSON.parse(localStorage.getItem('details_project'));
    if (object === null){
        return;
    }
    document.querySelector('.goal_project_input').value = object.goal_project_input || '';
    document.querySelector('.result_project_input').value = object.result_project_input || '';
    document.querySelector('.criteria_project_input').value = object.criteria_project_input || '';
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