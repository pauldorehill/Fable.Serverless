// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

var path = require("path");

module.exports = {
    mode: "development",
    entry: "./src/FableApp.fsproj",
    output: {
        path: path.join(__dirname, "../FunctionApp/public"),
        filename: "bundle.js"
    },
    devServer: {
        contentBase: "../FunctionApp/public",
        port: 8080
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    },
    resolve: {
        "alias": {
            "react": "preact/compat",
            "react-dom/test-utils": "preact/test-utils",
            "react-dom": "preact/compat"
        }
    }
}