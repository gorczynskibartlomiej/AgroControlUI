function setLeaveUrl(url) {
    var form = document.getElementById('leaveForm');
    form.action = url;
}
var leaveModal = new bootstrap.Modal(document.getElementById('leaveModal'), {
    keyboard: false
});
