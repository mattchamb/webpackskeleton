/// <reference path="../typings/tsd.d.ts" />

import http = require("http");

import express = require('express');
import serveStatic = require('serve-static');

var app = express();

app.use(serveStatic(__dirname + '/public'));

app.get('/api/hello', function (req, res) {
    res.send('Hello from Node!');
});

var port = process.env.port || 8080;

console.log('Server starting on port ' + port);
app.listen(port);