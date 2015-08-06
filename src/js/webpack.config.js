var webpack = require('webpack');
var ExtractTextPlugin = require("extract-text-webpack-plugin");

var webpackDevServerPort = 9090;
var publicPath = "http://localhost:" + webpackDevServerPort + "/";
var bundleName = "[name].js";

module.exports = {
    entry: {
        main: "./client/main.jsx"
    },
    output: {
        path: "../../bin/Debug/public",
        publicPath: publicPath,
        filename: bundleName
    },

    resolve: {
        extensions: ['', '.ts', '.webpack.js', '.web.js', '.js', '.jsx']
    },

    // Source maps support (or 'inline-source-map' also works)
    devtool: 'inline-source-map',

    module: {
        loaders: [
            {
                test: /\.ts$/,
                loader: 'awesome-typescript-loader'
            },
            {
                test: /\.jsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel'
            },
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract("style-loader", "css-loader!autoprefixer-loader?browsers=last 2 version")
            },
            {
                test: /\.less$/,
                loader: ExtractTextPlugin.extract("style-loader", "css-loader!autoprefixer-loader?browsers=last 2 version!less-loader")
            }
        ]
    },

    plugins: [
        new webpack.optimize.CommonsChunkPlugin("commons", "commons.js"),
        new ExtractTextPlugin("[name].css")
    ],

    devServer: {
        contentBase: "../../bin/Debug/public",
        port: webpackDevServerPort
    }
};