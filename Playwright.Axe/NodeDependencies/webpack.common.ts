
import path from "path";
import { Configuration } from "webpack";
import { ESBuildMinifyPlugin } from "esbuild-loader";

const enum EntryChunks {
    AxeCore = "axe-core",
    Report = "report",
    Lib = "lib"
}

const commonConfig: Configuration = {
    mode: "production",
    output: {
        path: path.join(__dirname, "dist"),
        filename: "index.[name].js",
    },
    resolve: {
        extensions: [".js", ".ts", ".tsx"]
    },
    performance: {
        hints: false
    },
    cache: {
        type: "filesystem"
    },
    optimization: {
        minimizer: [new ESBuildMinifyPlugin()]
    },
    module: {
        rules: [
            {
                test: /\.(ts|tsx)?$/,
                exclude: /node_modules/,
                use: "swc-loader",
            }
        ]
    }
}

export { commonConfig, EntryChunks };