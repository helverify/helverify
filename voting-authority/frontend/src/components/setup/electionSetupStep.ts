export class ElectionSetupStep {
    /**
     * Name of this step.
     */
    caption: string;

    /**
     * Component to be displayed for this step.
     */
    component: any;

    constructor(caption: string, component: any) {
        this.caption = caption;
        this.component = component;
    }
}