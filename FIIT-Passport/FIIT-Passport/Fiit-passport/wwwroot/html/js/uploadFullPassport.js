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
    updateField(`[name='name_customer']`, data.ordererName);
    updateField(`[name='name_project']`,  data.projectName);
    updateField(`[name='description_project']`, data.projectDescription);
    updateField(`[name='name']`, data.name);
    updateField(`[name='goal_project']`, data.goal);
    updateField(`[name='result_project']`, data.result);
    updateField(`[name='criteria_project']`, data.acceptanceCriteria);
    updateField(`[name='max']`, data.copiesNumber);
    updateField(`[name='address']`, data.meetingLocation);
    updateField(`[name='last_name']`, data.surname);
    updateField(`[name='telegram']`, data.telegramTag);
    updateField(`[name='email']`, data.email);
    updateField(`[name='phone']`, data.phoneNumber);
});

function updateField(fieldName, value){
    let field = document.querySelector(fieldName);
    if (field instanceof HTMLInputElement){
        field.value = value;
    } else{
        field.textContent = value;
    }
}