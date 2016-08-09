var ant_class_name = $("#ant_class_name");
var ant_class_attribute = $("#ant_class_attribute");
var submit = $("#submit");
submit.click(function () {
    if (ant_class_name.val() == "" || ant_class_name.val() == null) {
        alert("请输入区域名称");
        return false;
    }
    if (ant_class_attribute.val() == "" || ant_class_attribute.val() == null) {
        alert("请输入城市编码");
        return false;
    }
});