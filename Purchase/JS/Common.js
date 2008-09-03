
function include(file) {
  var script = document.createElement('script');

  script.src = file;
  script.type = 'text/javascript';
  script.defer = 'defer';

  document.getElementsByTagName('head')[0].appendChild(script);
}

//include('/Purchase/JS/Colorful.js');


function navi(menu) {
  if(top.header){
    var doc = top.header.document;

    doc.getElementById('home').className = '';
    doc.getElementById('product').className = '';
    doc.getElementById('supplier').className = '';
//    doc.getElementById('product_type').className = '';
//    doc.getElementById('rfq_status').className = '';
//    doc.getElementById('po_status').className = '';
    doc.getElementById('setting').className = '';
//    doc.getElementById('personal_setting').className = '';

    doc.getElementById(menu).className = 'current';
  }

  return true;
}

function popup(file) {
  var option = "width=" + 600 + ",height=" + 500;

  window.open(file, "" ,option + ",left=100,top=100,scrollbars=yes,menubar=no,toolbar=no,statusbar=no,resizable=yes,directories=no");
}

function clearForm(formname){

  var name = formname;
  var targetForm = document.forms[name];
  var len = targetForm.elements.length;

  for(i = 0; i < len; i++) {
    if(targetForm.elements[i].type == "text" || targetForm.elements[i].type == "textarea" || targetForm.elements[i].type == "password"){
      if(targetForm.elements[i].readOnly) continue;
      targetForm.elements[i].value = "";
    }
  }
}

function changeCellColor(tableid){
  if (!document.getElementById(tableid)) return false;
  var table = document.getElementById(tableid);

  if (!table.getElementsByTagName('TR')) return false;
  var tr = table.getElementsByTagName('TR')

  for(var no=0;no<tr.length;no++){
    if (tr[no].getElementsByTagName('TD')) {
        tr[no].onmouseover = overCell;
        tr[no].onmouseout = outCell;
    }
  }
}

function overCell(){
  var td = this.getElementsByTagName('TD')
  for(var no=0;no<td.length;no++){
    td[no].className = 'over';
  }
}

function outCell(){
  var td = this.getElementsByTagName('TD')
  for(var no=0;no<td.length;no++){
    td[no].className = '';
  }
}