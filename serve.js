
console.log('\033[2J');

var connect = require('connect');
var serveStatic = require('serve-static');
var publicRoot = __dirname + '/public/';
var port = 9995;
connect().use(serveStatic(publicRoot)).listen(port);
console.log("Serving up jsMate at http://localhost:" + port + "/");