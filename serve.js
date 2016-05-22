
console.log('\033[2J');

var connect = require('connect');
var serveStatic = require('serve-static');
var publicRoot = __dirname + '/public/'
connect().use(serveStatic(publicRoot)).listen(1033);
console.log("Serving up jsMate at http://localhost:1033/");