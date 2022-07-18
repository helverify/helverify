import {BallotFactory} from "../factory/ballotFactory";
import {CastBallot} from "../ballot/castBallot";

describe("CastBallot test", () => {
   it("should contain selections", () => {
        // arrange, act
       const castBallot: CastBallot = BallotFactory.createCastBallot([null, null, null, ["d4", "a2", "ff"]]);

       // assert

       expect(castBallot.selection).toContain("d4");
       expect(castBallot.selection).toContain("a2");
       expect(castBallot.selection).toContain("ff");
    });
})