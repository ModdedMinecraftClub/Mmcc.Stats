<script lang="ts">
    import { onDestroy, onMount } from "svelte";

    // any because there are no typings for plotly :(;
    export let data: any;
    // default values, can be overwritten by parent;
    export let layout: any = {
        paper_bgcolor: "#334155",
        plot_bgcolor: "#334155",
        font: {
            family: "Inter",
            color: "#f1f5f9",
        },
        yaxis: {
            gridcolor: "#475569",
        },
        xaxis: {
            gridcolor: "#475569",
        },
        showlegend: true,
        legend: {
            orientation: "h",
            y: 1.5,
        },
    };
    // default values, can be overwritten by parent;
    export let config: any = { responsive: true };

    let plotDiv: HTMLDivElement;

    onMount(() => {
        // we ignore because TS linter doesn't know that we import plotly in index.html because npm package is broken so shows false errors;
        //@ts-ignore
        Plotly.newPlot(plotDiv, data, layout, config);
    });

    onDestroy(() => {
        // we ignore because TS linter doesn't know that we import plotly in index.html because npm package is broken so shows false errors;
        //@ts-ignore
        Plotly.purge(plotDiv);
    });
</script>

<div bind:this={plotDiv} />
