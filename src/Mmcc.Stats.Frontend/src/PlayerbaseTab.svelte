<script>
    import { fade } from 'svelte/transition';

    let chart;
    let from;
    let to;
    let selectedMode;

    let modes = [
        { id: 0, text: "Smoothed data" },
        { id: 1, text: "Raw data" }
    ]

    async function handleClick() {
        if (from === '' || to === '' || from === undefined || to === undefined) {
            alert("Date cannot be empty");
            return;
        }
    
        if (from > to) {
            alert("From date cannot be bigger than To date");
            return;
        }

        let response = await fetch(`/api/playerbase-stats?from=${from}&to=${to}`);

        if (!response.ok) {
            alert("API HTTP-Error" + response.status);
            return;
        }

        let responseData = await response.json();

        createPlot(responseData);
    }

    function createPlot(data) {
        let traces = [];
    
        for (const serverData of data) {
            let parallelArrays = createParallelArrays(serverData.pings);

            if (selectedMode.id == 0) {
                traces.push(createSmoothTrace(serverData.serverName, parallelArrays));
            } else {
                traces.push(createRawTrace(serverData.serverName, parallelArrays));
            }
        }

        let layout = {
            autosize: true // set autosize to rescale
        };    
        let config = {responsive: true}

        Plotly.newPlot('plot', traces, layout, config);    
    }

    function createSmoothTrace(name, parallelArrays) {
        return {
            name: name,
            x: parallelArrays.times,
            y: smooth(parallelArrays.players, 30),
            mode: 'lines',
            type: 'scatter'
        }
    }

    function createRawTrace(name, parallelArrays) {
        return {
            name: name,
            x: parallelArrays.times,
            y: parallelArrays.players,
            mode: 'lines',
            type: 'scatter'
        };
    }

    function createParallelArrays(pings) {
        let times = [];
        let players = [];

        for (const ping of pings) {
            times.push(ping.time);
            players.push(ping.playersOnline);
        }

        return {
            times,
            players
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
            <span class="select">
                <select bind:value={selectedMode}>
                    {#each modes as mode}
                        <option value={mode}>
                            {mode.text}
                        </option>
                    {/each}
                </select>
            </span>
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