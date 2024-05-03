function getCookie(name) {
    const cookie = document.cookie.match(new RegExp(name + '=(.+?)(;|$)'));
    if (cookie === null) {
        return undefined;
    }
    return cookie[1];
}

async function CheckAuthenticate() {
    await fetch('http://51.250.123.70:8888/api/passport/authenticate',
        {
            method: 'POST',
            headers: {
                'Content-Type' : 'application/json'
            },
            body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value,
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
            })
        }).then(response => {
        return response.json();
    }).then(data => {
        if (data.state === "error") {
            toastr.error(data.message);
        }
        else{
            toastr.success(data.message);
        }
        console.log(data.state);
    });
}

async function SubmitPassport() {
    const form = document.forms[0];
    if (!form.checkValidity()){
        form.reportValidity();
        return false;
    }
    await fetch('http://51.250.123.70:8888/api/passport/confirm',
        {
            method: 'POST',
            headers: {
                'Content-Type' : 'application/json'
            },
            body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value,
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
            })
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось пройти процесс аутентификации');
        }
        return response.json();
    }).then(data => {
        if (data.message === "Ok") {
            document.getElementById("controller_button_send").click();
        }
        else {
            toastr.error(data.message);
        }
    });
}