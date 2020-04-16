"use strict";

const { VueLoaderPlugin } = require('vue-loader');
var webpack = require('webpack');

module.exports = {
    mode: 'development',
    entry: {
        main: './scripts/entryPoints/index.js',
        news: './scripts/entryPoints/news.js',
        documents: './scripts/entryPoints/documents.js',
        people: './scripts/entryPoints/people.js',
        calendar: './scripts/entryPoints/calendar.js',
        addNews: './scripts/entryPoints/addNews.js',
        error: './scripts/entryPoints/error.js'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                use: 'vue-loader'
            },
            {
                resourceQuery: /blockType=i18n/,
                type: 'javascript/auto',
                loader: '@kazupon/vue-i18n-loader'
            },
            {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            },
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        }),
        new VueLoaderPlugin()
    ],
    externals: {
        vuei18n: 'vue-i18n',
        portal: 'portal'
    }
};