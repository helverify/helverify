import {ElectionDto} from "../../api/Api";

export class ElectionInfoProps {
    election: ElectionDto;

    constructor(election:ElectionDto) {
        this.election = election;
    }
}