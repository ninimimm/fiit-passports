function uploadInputFields(data) {
    document.querySelector('.name_customer_input').value = data.ordererName;
    document.querySelector('.name_project_input').value = data.projectName;
    document.querySelector('.description_project_input').value = data.projectDescription;
    document.querySelector('.name_input').value = data.name;
    document.querySelector('.goal_project_input').value = data.goal;
    document.querySelector('.result_project_input').value = data.result;
    document.querySelector('.criteria_project_input').value = data.acceptanceCriteria;
    document.querySelector('.max_input').value = data.copiesNumber;
    document.querySelector('.address_input').value = data.meetingLocation;
    document.querySelector('.last_name_input').value = data.surname;
    document.querySelector('.telegram_input').value = data.telegramTag;
    document.querySelector('.email_input').value = data.email;
    document.querySelector('.phone_input').value = data.phoneNumber;
}

function uploadFromSorage(data) {
    document.querySelector('.name_customer_input').value = data.OrdererName;
    document.querySelector('.name_project_input').value = data.ProjectName;
    document.querySelector('.description_project_input').value = data.ProjectDescription;
    document.querySelector('.name_input').value = data.Name;
    document.querySelector('.goal_project_input').value = data.Goal;
    document.querySelector('.result_project_input').value = data.Result;
    document.querySelector('.criteria_project_input').value = data.AcceptanceCriteria;
    document.querySelector('.max_input').value = data.CopiesNumber;
    document.querySelector('.address_input').value = data.MeetingLocation;
    document.querySelector('.last_name_input').value = data.Surname;
    document.querySelector('.telegram_input').value = data.TelegramTag;
    document.querySelector('.email_input').value = data.Email;
    document.querySelector('.phone_input').value = data.PhoneNumber;
}

function createSendObject(){
    return { SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value, 
    ProjectName: document.querySelector('.name_project_input').value,   
    ProjectDescription: document.querySelector('.description_project_input').value,
    Goal: document.querySelector('.goal_project_input').value, 
    Result: document.querySelector('.result_project_input').value,   
    AcceptanceCriteria: document.querySelector('.criteria_project_input').value,
    CopiesNumber: document.querySelector('.max_input').value, 
    MeetingLocation: document.querySelector('.address_input').value, 
    Name: document.querySelector('.name_input').value,
    Surname: document.querySelector('.last_name_input').value,
    TelegramTag: document.querySelector('.telegram_input').value,
    Email: document.querySelector('.email_input').value,
    PhoneNumber: document.querySelector('.phone_input').value,
    };
}

async function getCheckPassport() {
    await getPassport().then(data => uploadInputFields(data));
}

async function Update() {
    await updatePassport(createSendObject());
} 

async function CheckAuthenticate() {
    await authenticate(createSendObject()).then(response => {
        return response.json();
    }).then(data => {
        if (data.state === "error") {
            toastr.error(data.message);
        }
        else{
            toastr.success(data.message);
        }
    });
}

async function SubmitPassport() {
    const form = document.forms[0];
    if (!form.checkValidity()){
        form.reportValidity();
        return false;
    }
    await confirm(createSendObject()).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось пройти процесс аутентификации');
        }
        return response.json();
    }).then(data => {
        if (data.message === "Ok") {
            window.location.href = '../request_send.html';
        }
        else {
            toastr.error(data.message);
        }
    });
}

function saveFields() {
    localStorage.setItem('check_request', JSON.stringify(createSendObject()));
}

function uploadFields() {
    const object = JSON.parse(localStorage.getItem('check_request'));
    if (object === null){
        let object = JSON.parse(localStorage.getItem('about_project'));
        if (object !== null){
            document.querySelector('.name_customer_input').value = object.name_customer_input || '';
            document.querySelector('.name_project_input').value = object.name_project_input || '';
            document.querySelector('.description_project_input').value = object.description_project_input || '';
        }
        object = JSON.parse(localStorage.getItem('details_project'));
        if (object !== null){
            document.querySelector('.goal_project_input').value = object.goal_project_input || '';
            document.querySelector('.result_project_input').value = object.result_project_input || '';
            document.querySelector('.criteria_project_input').value = object.criteria_project_input || '';
        }
        object = JSON.parse(localStorage.getItem('others_project'));
        if (object !== null){
            document.querySelector('.max_input').value = object.max_input || '';
            document.querySelector('.address_input').value = object.address_input || '';
        }
        object = JSON.parse(localStorage.getItem('contacts'));
        if (object !== null){
            document.querySelector('.name_input').value = object.name_input || '';
            document.querySelector('.last_name_input').value = object.last_name_input || '';
            document.querySelector('.telegram_input').value = object.telegram_input || '';
            document.querySelector('.email_input').value = object.email_input || '';
            document.querySelector('.phone_input').value = object.phone_input || '';
        }
        return;
    }
    uploadFromSorage(object);
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