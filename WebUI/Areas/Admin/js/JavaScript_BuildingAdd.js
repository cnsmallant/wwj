var ant_class = $("#ant_class");
var ant_trunk_class_id = $("#ant_trunk_class_id");
var ant_trunk_code = $("#ant_trunk_code");
var ant_trunk_title = $("#ant_trunk_title");
var ant_trunk_entitle = $("#ant_trunk_entitle");
var ant_trunk_summary = $("#ant_trunk_summary");
var ant_content_tags = $("#ant_content_tags");
var ant_trunk_area = $("#ant_trunk_area");
var ant_trunk_order = $("#ant_trunk_order");
var ant_trunk_content = $("#ant_trunk_content");
var submit = $("#submit");
var g = $("#g");
var s = $("#s");
var c = $("#c");
var flg = false;
ant_class.change(function () {
    if (ant_class.find("option:selected").text() == "请选择") {
        flg = false;
    } else {
        var ro = ant_class.find("option:selected").val();

        ant_trunk_class_id.val(ro);
        flg = true;
    }
});
g.click(function () {
    ant_trunk_area.val(g.text());
});
s.click(function () {
    ant_trunk_area.val(s.text());
});
c.click(function () {
    ant_trunk_area.val(c.text());
});
submit.click(function () {
    if (flg == false) {
        alert("请选择建筑区域");
        return false;
    }
    if (ant_trunk_area.val() == null || ant_trunk_area.val() == "") {
        alert("请输入保护级别");
        return false;
    }
    if (ant_trunk_code.val() == null || ant_trunk_code.val() == "") {
        alert("请输入建筑编号");
        return false;
    }
    if (ant_trunk_title.val() == null || ant_trunk_title.val() == "") {
        alert("请输入中文名称");
        return false;
    }
    //if (ant_trunk_entitle.val() == null || ant_trunk_entitle.val() == "") {
    //    alert("请输入外文名称");
    //    return false;
    //}
    if (ant_content_tags.val() == null || ant_content_tags.val() == "") {
        alert("请输入建筑地址");
        return false;
    }
    if (ant_trunk_code.val() == null || ant_trunk_code.val() == "") {
        alert("请输入建造时间");
        return false;
    }
   
    if (ant_trunk_order.val() == null || ant_trunk_order.val() == "") {
        alert("请输入建筑序号");
        return false;
    }
    if (ant_trunk_content.val() == null || ant_trunk_content.val() == "") {
        alert("请输入建筑描述");
        return false;
    }
});