import type { SvelteComponent } from "svelte";

export interface IMenuItemContent {
    name: string,
    href: string,
};

export interface IMenuButtonContent extends IMenuItemContent {
    icon: any,
    colourName: string,
};