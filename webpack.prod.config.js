var webpack = require('webpack');
var objectAssign = require('object-assign');
var ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = objectAssign(require('./webpack.config'), {
    output: {
        path: "./build/Release/public",
        filename: "[name].js"
    },
    plugins: [
        new webpack.DefinePlugin({
            "process.env": {
                NODE_ENV: JSON.stringify("production")
            }
        }),
        new webpack.optimize.OccurenceOrderPlugin(true),
        new webpack.NoErrorsPlugin(),
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            }
        }),
        new webpack.optimize.CommonsChunkPlugin("commons", "commons.js"),
        new ExtractTextPlugin("[name].css")
    ]
});
