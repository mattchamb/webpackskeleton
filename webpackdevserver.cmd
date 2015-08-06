@ECHO OFF
cd %~dp0src\js
webpack-dev-server --config webpack.config.js
