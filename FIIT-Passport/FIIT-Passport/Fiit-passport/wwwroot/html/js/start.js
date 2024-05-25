async function create() {
    const response = await fetch('http://51.250.123.70:8888/api/passport/create',
        {
            method: 'POST'
        }).then(response => {
            if (!response.ok) {
                throw new Error('Не получилось создать паспорт');
            }
            return response.json();
    }).then(data => { 
        let currentDate = new Date();
        currentDate.setFullYear(currentDate.getFullYear() + 10);
        const date = currentDate.toUTCString();
        document.cookie = `idSession=${data.SessionId}; expires=${date}`;
        window.location.href = 'about_project.html';
    });
}

if (getCookie("idSession") !== undefined) {
    fetch('http://51.250.123.70:8888/api/passport/get',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ sessionId: getCookie("idSession") })
    }).then(response => {
    if (!response.ok) {
        throw new Error('Не получилось получить паспорт');
    }
    return response.json();
    }).then(data => {
            if (data.status === 0) {
                let lastForm = document.querySelector('.fill_form');
                var buttonContainer = document.createElement("div");
                buttonContainer.innerHTML = '<form action="about_project.html"><button type="submit" class="fill_form" style="margin-left: 5vw; margin-right: -1vw">Изменить анкету</button></form>';
                lastForm.parentNode.insertBefore(buttonContainer.firstChild, lastForm.nextSibling);
        };
    });
}
