import type axe from "axe-core";
import type { ElementContext, RunOptions, PartialResult } from "axe-core";

type AxeType = typeof axe;

/**
 * Recursively calls Axe runPartial in each frame and adds it to an array.
 */
function runPartialRecursive(context: ElementContext, options: RunOptions, win = window) : Promise<PartialResult>[] {
    const axe = win["axe"] as AxeType;

    if(axe === null || axe === undefined)
    {
      throw new Error("Axe not initialized in window.")
    }

    // Find all frames in context, and determine what context object to use in that frame
    const frameContexts = axe.utils.getFrameContexts(context, options);

    // Run the current context, in the current window.
    const promiseResults = getRunPartialResults(axe, context, options);

    // Loop over all frames in context
    frameContexts.forEach(({ frameSelector, frameContext }) => {
      // We need to get the window of the frame.
      // Axe should be initialized on this window already.
      const frame: HTMLIFrameElement = axe.utils.shadowSelect(frameSelector) as HTMLIFrameElement;
      const frameWin = frame.contentWindow as Window & typeof globalThis;

      // Call axe.runPartial() in the frame, and its child frames
      const frameResults = runPartialRecursive(frameContext, options, frameWin);
      promiseResults.push(...frameResults);
    });

    return promiseResults;
}

/**
 * Executes run partial and puts the results into an array.
 * If there is an issue in executing run partial then null is added to the array:
 * https://github.com/dequelabs/axe-core/blob/master/doc/run-partial.md#axefinishrunpartialresults-options-promise
 */
function getRunPartialResults(axe: AxeType, context: ElementContext, options: RunOptions) : Promise<PartialResult>[]
{
  try {
    return [axe.runPartial(context, options)];
  } catch
  {
    return [null]
  }
}

/**
 * Runs Axe runPartial in each frame and merges the results into a flattened array.
 */
function runPartialAndMerge(context: ElementContext, options: RunOptions, win = window) : Promise<PartialResult[]>
{
  return Promise.all(runPartialRecursive(context, options, win))
    .then(result => result.flat(1));
}

export { runPartialAndMerge }