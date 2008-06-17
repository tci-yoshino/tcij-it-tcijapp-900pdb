
var colorful = new ColorfulInput;
var cellColor = new CellColorChange;
colorful.skip = ['submit','button','reset'];
colorful.color['focus'] = '#FFF799';

window.onload = function() {
   colorful.set();
   cellColor.set();
}

function ColorfulInput() {
   this.skip  = [];
   this.color = { 'blur': '', 'focus': '#EEEEEE' };

   this.set = function() {
      for (var i = 0; i < document.forms.length; i++) {
         for (var f = 0; f < document.forms[i].length; f++) {
            var elm = document.forms[i][f];
            if(!this._checkSkip(elm)) continue;

            this._setColor(elm, 'focus');
            this._setColor(elm, 'blur');
         }
      }
   }

   this._checkSkip = function(elm) {
      for(var i in this.skip) {
         if(elm.type == this.skip[i]) return false;
      }
      if(elm.readOnly) return false;
      return true;
   }

   this._setColor = function(elm, type) { 
      var color = this.color[type];
      var event = function() { elm.style.backgroundColor = color; };

      if(elm.addEventListener) {
         elm.addEventListener(type, event, false);
      } else if(elm.attachEvent) {
         elm.attachEvent('on'+type, event);
      } else {
         elm['on'+type] = event;
      }
   }
}

function CellColorChange(){

  this.set = function() {
    if (document.getElementsByTagName('TABLE')){
      var tableObj = document.getElementsByTagName('TABLE');
      for(var no=0;no<tableObj.length;no++){
        if(tableObj[no].parentNode.className == "list") {
          var rows = tableObj[no].getElementsByTagName('TR');
          for(var rowno=0;rowno<rows.length;rowno++){
            rows[rowno].onmouseover = this._over;
            rows[rowno].onmouseout  = this._out;
          }
        }
      }
    }
  }
  
  this._over = function (){
    var tdrows = this.getElementsByTagName('TD');
    for(var tno=0;tno<tdrows.length;tno++){
      tdrows[tno].className = "over";
    }
  }

  this._out = function (){
    var tdrows = this.getElementsByTagName('TD');
    for(var tno=0;tno<tdrows.length;tno++){
      tdrows[tno].className = "";
    }
  }
  
}

