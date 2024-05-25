function getCookie(name) {
    const cookie = document.cookie.match(new RegExp(name + '=(.+?)(;|$)'));
    if (cookie === null) {
        return undefined;
    }
    return cookie[1];
}