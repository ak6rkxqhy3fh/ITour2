$(document).ready(function () {
    var container = $('[sortable-container]');
    container.sortable({
        items: "[sortable-item]",
        update: function () {
            var sortableResult = container.sortable('toArray', {
                attribute: 'sortable-item'
            });
            document.getElementById("sortableResult").value = sortableResult;
        }
    });
});