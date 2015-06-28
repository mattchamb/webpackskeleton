var HtmlWebpackPlugin = require('html-webpack-plugin');
var webpack = require('webpack');
var objectAssign = require('object-assign');

var bundleName = "bundle.js";

module.exports = objectAssign(require('./webpack.config'), {
    output: {
        path: "./build/public",
        filename: bundleName
    },
    plugins: [
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './client/index-template.html',
            bundleLocation: bundleName
        }),
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            }
        }),
        new webpack.optimize.OccurenceOrderPlugin(true)
    ]
});
