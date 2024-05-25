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
    document.querySelector('.max_input').value = data.copiesNumber;
    document.querySelector('.address_input').value = data.meetingLocation;
});

async function UpdatePassport() {
    await fetch('http://51.250.123.70:8888/api/passport/update',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), CopiesNumber: document.querySelector('.max_input').value, 
                                MeetingLocation: document.querySelector('.address_input').value,   
        })
    });
}