// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

const path = require("path");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    mode: "development",
    entry: "./src/FableApp.fsproj",
    output: {
        path: path.join(__dirname, "../FunctionApp/public"),
        filename: "bundle.[contenthash].js"
    },
    plugins: [
        new CleanWebpackPlugin(),
        new HtmlWebpackPlugin({
            template: "./src/public/index.html",
            favicon: "./src/public/favicon.ico",
            title: "Fable + Preact + Serverless",
            hash: true,
            cache: true
        }),
    ],
    devServer: {
        contentBase: "../FunctionApp/public",
        port: 8080
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: "fable-loader"
            }
        ]
    },
    resolve: {
        "alias": {
            "react": "preact/compat",
            "react-dom/test-utils": "preact/test-utils",
            "react-dom": "preact/compat"
        }
    }
}