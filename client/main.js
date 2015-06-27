require('./styles/hello.css');
require('./styles/hello.less');

document.write('Hello from JS! <br/>');
document.writeln(require('./lib/hello.ts'));

function reqListener () {
    var text = document.createTextNode(this.responseText);
    document.body.appendChild(text);
}

var oReq = new XMLHttpRequest();
oReq.onload = reqListener;
oReq.open('get', "/api/hello", true);
oReq.send();