
console.log('\033[2J');

var connect = require('connect');
var serveStatic = require('serve-static');
connect().use(serveStatic(__dirname)).listen(1033);
console.log("Serving up jsMate at http://localhost:1033/");