
import path from "path";
import { Configuration } from "webpack";
import { merge } from "webpack-merge"
import { commonConfig, EntryChunks } from "./webpack.common";

const libConfig: Configuration = {
    entry: {
        [EntryChunks.Lib]: path.join(__dirname, "./src/axe/index.lib.ts")
    },
    output: {
        library: {
            type: "var",
            name: "exposedModules"
        }
    }
}

export default merge(libConfig, commonConfig);