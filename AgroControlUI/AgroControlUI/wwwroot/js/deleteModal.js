function setDeleteUrl(url) {
    var form = document.getElementById('deleteForm');
    form.action = url;
}
var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'), {
    keyboard: false
});