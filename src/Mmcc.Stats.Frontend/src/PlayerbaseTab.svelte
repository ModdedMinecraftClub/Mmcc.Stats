<script>
    import { fade } from 'svelte/transition';

    let chart;
    let from;
    let to;
    let selectedMode;
    let loading = false;

    let modes = [
        { id: 0, text: "Daily rolling average" },
        { id: 1, text: "Weekly rolling average" },
        { id: 2, text: "Raw data"}
    ]

    async function handleClick() {
        document.getElementById("plot").innerHTML = "";
        loading = true;

        if (from === '' || to === '' || from === undefined || to === undefined) {
            alert("Date cannot be empty");
            return;
        }
    
        if (from > to) {
            alert("From date cannot be bigger than To date");
            return;
        }

        let response;

        if (selectedMode.id == 0) {

            response = await fetch(`/api/playerbase/chart/avg?from=${from}&to=${to}&windowSize=138`);
        } else if (selectedMode.id == 1) {
            response = await fetch(`/api/playerbase/chart/avg?from=${from}&to=${to}&windowSize=966`);
        } else if (selectedMode.id == 2) {
            response = await fetch(`/api/playerbase/chart?from=${from}&to=${to}`);
        } else {
            throw new RangeError("selectedMode out of range");
        }

        if (!response.ok) {
            alert("API HTTP-Error" + response.status);
            return;
        }

        let responseData = await response.json();

        await createPlot(responseData);
    }

    async function createPlot(data) {
        let traces = [];
    
        for (const serverData of data) {
            traces.push(createRawTrace(serverData));
        }

        let layout = {
            autosize: true // set autosize to rescale
        };    
        let config = {responsive: true};

        loading = false;

        setTimeout(() => {  Plotly.newPlot("plot", traces, layout, config); }, 400);    
    }

    function createRawTrace(data) {
        return {
            name: data.serverName,
            x: data.times,
            y: data.players,
            mode: 'lines',
            type: 'scatter'
        };
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

{#if loading}
    <div id="loading" transition:fade wmode="transparent">
        <p>Loading...</p>
    </div>
{/if}

<div id="plot" transition:fade></div>

<style>
    form {
        padding: 0;
        display: flex;
        justify-content: center;
        max-width: 100%;
        max-height: 100%;
    }

    #loading {
        text-align: center;
        margin-top: 8%;
    }

    #plot {
        margin:auto;
        max-width: 90%;
        max-height: 99%;
    }
</style>