function GetComments() {
    return fetch('http://localhost:8888/api/comment/get',
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ sessionId: getCookie("idSession")})
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось получить комментарии');
        }
        return response.json();
    });
}