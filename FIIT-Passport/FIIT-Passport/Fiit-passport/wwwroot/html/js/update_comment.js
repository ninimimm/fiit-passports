function UpdateComment(id, text) {
    console.log(text);
    fetch('http://localhost:8888/api/comment/update',
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ id: `${id}`,
                text: text
                })
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось обновить текст коммента');
        }
        return response;
    });
}