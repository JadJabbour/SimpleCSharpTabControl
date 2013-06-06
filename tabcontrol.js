function ShowTab(tabTitleCell, idx) {
    var tabsTable = tabTitleCell.parentElement.parentElement.parentElement;
    var activeTabIdx = Number(tabsTable.getAttribute("ActiveTabIdx"));
    tabsTable.rows[0].cells[activeTabIdx].style["backgroundColor"] = "inactiveborder";
    tabsTable.rows[0].cells[idx].style["backgroundColor"] = "darkgray";
    tabsTable.rows[activeTabIdx + 1].style.display = "none";
    tabsTable.rows[idx + 1].style.display = "";
    tabsTable.setAttribute("ActiveTabIdx", idx);
}