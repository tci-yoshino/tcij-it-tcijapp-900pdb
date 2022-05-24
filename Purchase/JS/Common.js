
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
    doc.getElementById('rfq_status').className = '';
    doc.getElementById('po_status').className = '';
    doc.getElementById('rfq_search').className = '';
    doc.getElementById('setting').className = '';
//    doc.getElementById('personal_setting').className = '';

    doc.getElementById(menu).className = 'current';
  }

  return true;
}

function popup(file) {
  var option = "width=" + 600 + ",height=" + 500;

  window.open(file, "" ,option + ",left=100,top=100,scrollbars=yes,menubar=no,toolbar=yes,location=yes,statusbar=no,resizable=yes,directories=no");
}

function clearForm(formname){

  var name = formname;
  var targetForm = document.forms[name];
  var len = targetForm.elements.length;

  for(i = 0; i < len; i++) {
    if(targetForm.elements[i].type === "text" || targetForm.elements[i].type === "textarea" || targetForm.elements[i].type === "password"){
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
        //tr[1].childNodes[4].className
    }
  }
}

function overCell(){
  var td = this.getElementsByTagName('TD')
  for (var no = 0; no < td.length; no++) {
      td[no].className = td[no].className + ' over';
  }
}

function outCell(){
    var td = this.getElementsByTagName('TD')
    var classname;
    for (var no = 0; no < td.length; no++) {
        classname = td[no].className.replace('over', '');
        td[no].className = classname;
  }
}

function setAction(action) {
    const hiddenElem = document.getElementById('Action');
    if (!hiddenElem) { console.error('id="Action" element does not exist. in ' + window.location.href.split('/').pop()); return false; }
    hiddenElem.value = action;
    return true;
}

function ListSort(hidden_sort_type, hidden_sort_field) {
    // 選択されたソート条件を表示する
    var sort_type = hidden_sort_type.value;
    var sort_field = hidden_sort_field.value;

    let ths = document.getElementsByTagName("th");
    for (var i = 0; i < ths.length; i++) {
        if (!ths[i].classList.contains('sortField')) {
            continue;
        }
        // 値比較
        if (ths[i].id === sort_field) {
            if (sort_type === 'asc') {
                ths[i].classList.add('asc')
            } else if (sort_type === 'desc') {
                ths[i].classList.add('desc')
            } else {
                ths[i].classList.add('asc')
            }
        }

        // マウスリーブ
        ths[i].onmouseleave = function(event){
            let element = event.target;
            if (element.id === sort_field) {
                if (element.classList.contains('asc')) {
                    element.classList.replace('asc', 'desc');
                } else if (element.classList.contains('desc')) {
                    element.classList.replace('desc', 'asc');
                } else {
                    element.classList.add('asc');
                }
            } else {
                element.classList.remove('asc', 'desc')
            }
        };

        // マウスオーバー
        ths[i].onmouseover = function (event) {
            let element = event.target;
            if (element.classList.contains('asc')) {
                element.classList.replace('asc', 'desc');
            } else if (element.classList.contains('desc')) {
                element.classList.replace('desc', 'asc');
            } else {
                element.classList.add('asc');
            }
            let sort_field = hidden_sort_field.value;
            Array.from(element.parentNode.children)
                .filter(function (e) { e !== element })
                .filter(function (e) { e.id !== sort_field })
                .forEach(function (e) { e.classList.remove('asc', 'desc') });
        };

        // クリック時
        ths[i].onclick = function (event) {
            let element = event.target;
            if (element.classList.contains('asc')) {
                hidden_sort_type.value = 'asc';
            } else if (element.classList.contains('desc')) {
                hidden_sort_type.value = 'desc';
            } else {
                hidden_sort_type.value = 'asc';
            }
            hidden_sort_field.value = element.id;
            setAction('');
            document.forms["PageForm"].submit();
        };
    };
}

