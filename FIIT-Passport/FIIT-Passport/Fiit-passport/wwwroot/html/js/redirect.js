async function Redirect(page) {
    const form = document.forms[0];
    if (!form.checkValidity()){
        form.reportValidity();
        return false;
    }
    await UpdatePassport();
    window.location.href = page;
    return false;
}