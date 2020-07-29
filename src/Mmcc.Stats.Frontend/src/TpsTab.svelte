<script>
    import { fade } from 'svelte/transition';

    let chart;
    let from;
    let to;

    async function handleClick() {
        if (from === '' || to === '' || from === undefined || to === undefined) {
            alert("Date cannot be empty");
            return;
        }
    
        if (from > to) {
            alert("From date cannot be bigger than To date");
            return;
        }

        let response = await fetch(`/api/tps-stats?from=${from}&to=${to}`);

        if (!response.ok) {
            alert("API HTTP-Error" + response.status);
            return;
        }

        let responseData = await response.json();

        console.log(responseData);

        createPlot(responseData);
    }

    function createPlot(data) {
        let traces = [];
    
        for (const serverData of data) {
            let parallelArrays = createParallelArrays(serverData.tpsStats);

            traces.push(createTrace(serverData.serverName, parallelArrays));
        }

        let layout = {
            autosize: true // set autosize to rescale
        };    
        let config = {responsive: true}

        console.log(traces);

        Plotly.newPlot('plot', traces, layout, config);    
    }

    function createTrace(name, parallelArrays) {
        return {
            name: name,
            x: parallelArrays.times,
            y: parallelArrays.tpsList,
            mode: 'lines',
            type: 'scatter',
            line: {
                smoothing: 1.3
            }
        };
    }

    function createParallelArrays(tpsStats) {
        let times = [];
        let tpsList = [];

        for (const tpsStat of tpsStats) {
            times.push(tpsStat.time);
            tpsList.push(tpsStat.tps);
        }

        console.log(times);
        console.log(tpsList);

        return {
            times,
            tpsList
        }
    }
</script>

<form>
    <div class="field has-addons">
        <p class="control">
            <a class="button is-static">From:</a>
        </p>
        <p class="control">
            <input class="input is-link datepicker" id="from" type="date" bind:value={from}>
        </p>
        <p class="control">
            <a class="button is-static">To:</a>
        </p>
        <p class="control">
            <input class="input is-link datepicker" id="to" type="date" bind:value={to}>
        </p>
        <p class="control">
            <a class="button is-link" on:click={handleClick}>Go</a>
        </p>
    </div>        
</form>
<div id="plot" transition:fade></div>

<style>
    form {
        padding: 0;
        display: flex;
        justify-content: center;
        max-width: 100%;
        max-height: 100%;
    }

    #plot {
        margin:auto;
        max-width: 90%;
        max-height: 99%;
    }
</style>