import {ElectionDto} from "../../Api";

export class ElectionInfoProps {
    election: ElectionDto;

    constructor(election:ElectionDto) {
        this.election = election;
    }
}