
function include(file) {
  var script = document.createElement('script');

  script.src = file;
  script.type = 'text/javascript';
  script.defer = 'defer';

  document.getElementsByTagName('head')[0].appendChild(script);
}

//include('/Purchase/JS/Colorful.js');


function navi(menu) {
  var doc = top.header.document;

  doc.getElementById('home').className = '';
  doc.getElementById('product').className = '';
  doc.getElementById('supplier').className = '';
//  doc.getElementById('product_type').className = '';
//  doc.getElementById('rfq_status').className = '';
//  doc.getElementById('po_status').className = '';
  doc.getElementById('setting').className = '';
//  doc.getElementById('personal_setting').className = '';

  doc.getElementById(menu).className = 'current';

  return true;
}

function popup(file) {
  var option = "width=" + 600 + ",height=" + 500;

  window.open(file, "popup" ,option + ",left=100,top=100,scrollbars=yes,menubar=no,toolbar=no,statusbar=no,resizable=yes,directories=no");
}
