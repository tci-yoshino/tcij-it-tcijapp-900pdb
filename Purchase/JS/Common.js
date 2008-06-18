
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

  window.open(file, "popup" ,option + ",left=100,top=100,scrollbars=yes,menubar=no,toolbar=no,statusbar=no,resizable=yes,directories=no");
}

function clearForm(formname){

  var name = formname;
  var targetForm = document.forms[name];
  var len = targetForm.elements.length;

  for(i = 0; i < len; i++) {
    if(targetForm.elements[i].type == "text" || targetForm.elements[i].type == "textarea" || targetForm.elements[i].type == "password"){
      targetForm.elements[i].value = "";
    }
  }
}

function ChangeClass(obj){
  if (!obj.getElementsByTagName('TD')) return false;
  var tdrows = obj.getElementsByTagName('TD');
    for(var tno=0;tno<tdrows.length;tno++){
      tdrows[tno].className = "over";
  }
}

function DllClass(obj){
  if (!obj.getElementsByTagName('TD')) return false;
  var tdrows = obj.getElementsByTagName('TD');
    for(var tno=0;tno<tdrows.length;tno++){
      tdrows[tno].className = "";
  }
}