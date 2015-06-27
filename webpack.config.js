var HtmlWebpackPlugin = require('html-webpack-plugin');

var webpackDevServerPort = 9090;
var publicPath = "http://localhost:" + webpackDevServerPort + "/";
var bundleName = "bundle.js";

module.exports = {
    entry: {
        app: ["./client/main.js"]
    },
    output: {
        path: "./build/public",
        publicPath: publicPath,
        filename: bundleName
    },

    resolve: {
        extensions: ['', '.ts', '.webpack.js', '.web.js', '.js', '.jsx']
    },

    // Source maps support (or 'inline-source-map' also works)
    devtool: 'source-map',

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
                loader: "style-loader!css-loader!autoprefixer-loader?browsers=last 2 version"
            },
            {
                test: /\.less$/,
                loader: "style-loader!css-loader!autoprefixer-loader?browsers=last 2 version!less-loader"
            }
        ]
    },

    plugins: [new HtmlWebpackPlugin({
        filename: 'index.html',
        template: './client/index-template.html',
        bundleLocation: publicPath + bundleName
    })],

    devServer: {
        contentBase: "./build/public",
        port: webpackDevServerPort
    }
};