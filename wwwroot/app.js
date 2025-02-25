function copy(e) {
    const url = location.protocol + '//' + location.host + '/' + e.target.getAttribute('data-id');
    navigator.clipboard.writeText(url);
}