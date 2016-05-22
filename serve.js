
console.log('\033[2J');

var cors = require('cors');
var connect = require('connect');
var serveStatic = require('serve-static');
var publicRoot = __dirname + '/public/';
var port = 9995;

var allowCrossDomain = function(req, res, next) {
    res.header('Access-Control-Allow-Origin', config.allowedDomains);
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE');
    res.header('Access-Control-Allow-Headers', 'Content-Type');

    next();
}

connect()
	//.use(cors())
	.use(serveStatic(publicRoot))
	.listen(port);

console.log("Serving up jsMate at http://localhost:" + port + "/");