async function DeleteComment(id) {
    return await fetch('http://localhost:8888/api/comment/delete',
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ id : `${id}`})
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось удалить комментарий');
        }
        return response;
    });
}