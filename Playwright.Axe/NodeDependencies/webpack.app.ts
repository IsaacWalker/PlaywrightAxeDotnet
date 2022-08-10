
import path from "path";
import { Configuration } from "webpack";
import { merge } from "webpack-merge"
import { commonConfig, EntryChunks } from "./webpack.common";
import HtmlWebpackPlugin from "html-webpack-plugin";

const appConfig: Configuration = {
    entry: {
        [EntryChunks.AxeCore]: path.join(__dirname, "./src/axe/index.axe-core.ts"),
        [EntryChunks.Report]: path.join(__dirname, "./src/report/index.tsx"),
    },
    
    plugins: [
        new HtmlWebpackPlugin({
            template: path.join(__dirname, "./src/report/index.html"),
            chunks: [EntryChunks.Report]
        })
    ]
}

export default merge(appConfig, commonConfig);