/**
 * Represents a cast ballot (including its selections)
 */
export class CastBallot {
    /**
     * Short codes of selected options / candidates
     */
    selection: string[];

    /**
     * Constructor
     * @param selection Short codes of selected options / candidates
     */
    constructor(selection: string[]) {
        this.selection = selection;
    }
}