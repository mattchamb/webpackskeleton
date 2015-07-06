var webpack = require('webpack');
var objectAssign = require('object-assign');

var bundleName = "bundle.js";

module.exports = objectAssign(require('./webpack.config'), {
    output: {
        path: "./build/Release/public",
        filename: bundleName
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            }
        }),
        new webpack.optimize.OccurenceOrderPlugin(true)
    ]
});
