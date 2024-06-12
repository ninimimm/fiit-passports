async function getContactsPassport() {
    await getPassport().then(data => {
        document.querySelector('.name_input').value = data.name;
        document.querySelector('.last_name_input').value = data.surname;
        document.querySelector('.telegram_input').value = data.telegramTag;
        document.querySelector('.email_input').value = data.email;
        document.querySelector('.phone_input').value = data.phoneNumber;
    })
}

async function Update() {
    await updatePassport({ SessionId: getCookie("idSession"),
        Name: document.querySelector('.name_input').value, 
        Surname: document.querySelector('.last_name_input').value,
        TelegramTag: document.querySelector('.telegram_input').value,
        Email: document.querySelector('.email_input').value,
        PhoneNumber: document.querySelector('.phone_input').value,
    });
}   

function saveFields() {
    const object = {
        name_input : document.querySelector('.name_input').value,
        last_name_input : document.querySelector('.last_name_input').value,
        telegram_input : document.querySelector('.telegram_input').value,
        email_input : document.querySelector('.email_input').value,
        phone_input : document.querySelector('.phone_input').value
    };
    localStorage.setItem('contacts', JSON.stringify(object));
}

function uploadFields() {
    const object = JSON.parse(localStorage.getItem('contacts'));
    if (object === null){
        return;
    }
    document.querySelector('.name_input').value = object.name_input || '';
    document.querySelector('.last_name_input').value = object.last_name_input || '';
    document.querySelector('.telegram_input').value = object.telegram_input || '';
    document.querySelector('.email_input').value = object.email_input || '';
    document.querySelector('.phone_input').value = object.phone_input || '';
}

window.onload = function() {
    uploadFields();
};

window.onbeforeunload = function() {
    saveFields();
};
