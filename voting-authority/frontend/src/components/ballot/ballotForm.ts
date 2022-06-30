export type BallotFormProps = {
    buttonCaption: string,
    buttonAction: (numberOfBallots: number) => void,
    isLoading: boolean
}