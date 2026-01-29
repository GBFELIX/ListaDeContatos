document.getElementById("checkAll").onclick = function () {
    var checkboxes = document.getElementsByClassName('checkbox-item');
    for (var checkbox of checkboxes) {
        checkbox.checked = this.checked;
    }
}
function ExportarExcel() {

}