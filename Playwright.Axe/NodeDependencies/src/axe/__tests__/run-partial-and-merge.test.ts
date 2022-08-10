import { PartialResult } from "axe-core";
import { runPartialAndMerge } from "../run-partial-and-merge";

beforeEach(() => {
    window["axe"] = undefined;
});

describe("Run Partial and Merge", () => {
    it("Runs when there are no child frames present.", async () => {

        const runPartialResult: PartialResult = {
            frames: [],
            results: [{
                id: "color-contrast",
                result: "inapplicable",
                pageLevel: true,
                impact: null,
                nodes: []
            }]
        };

        window["axe"] = {
            runPartial: jest.fn(() => runPartialResult),
            utils: {
                // No Child frames
                getFrameContexts: jest.fn(() => [])
            }
        }

        const results = await runPartialAndMerge(document, {}, window);

        expect(results.length).toBe(1);
        expect(results[0]).toBe(runPartialResult)
    });

    it("Runs and merges results from child frames in the expected order", async () => {
        const resultsSeed = [...Array(3).keys()];

        const resultForEachFrame = resultsSeed.map(id => {
            const runPartialResult: PartialResult = {
                frames: [],
                results: [{
                    id: id.toString(),
                    result: "inapplicable",
                    pageLevel: true,
                    impact: null,
                    nodes: []
                }]
            };

            return runPartialResult;
        })


        const windowImplementations = resultsSeed.map(i => ({
            axe: {
                runPartial: jest.fn((context) => resultForEachFrame[i]),
                utils: {
                    // No Child frames
                    getFrameContexts: jest.fn(() => i < resultsSeed.length ? [{ frameSelector: i + 1 }, {}] : []),
                    shadowSelect: jest.fn((frameSelector) => ({ contentWindow: windowImplementations[frameSelector] }))
                }
            }
        }));

        const results = await runPartialAndMerge(document, {}, windowImplementations[0]);

        expect(results[0]).toBe(resultForEachFrame[0]);
        expect(results[1]).toBe(resultForEachFrame[1]);
        expect(results[2]).toBe(resultForEachFrame[2]);
    })
})