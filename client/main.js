require('file?name=[name].[ext]!./index.html');
require('./styles/hello.css');
require('./styles/hello.less');

document.writeln('Hello from JS! <br/>');
document.writeln(require('./lib/hello.ts'));