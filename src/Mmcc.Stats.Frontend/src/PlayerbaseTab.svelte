<script>
    import { fade } from 'svelte/transition';

    let chart;
    let from;
    let to;
    let selectedMode;
    let loading = false;

    let modes = [
        { id: 0, text: "Smoothed data" },
        { id: 1, text: "Weekly rolling average" },
        { id: 2, text: "Raw data" }
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

        let response = await fetch(`/api/playerbase-stats?from=${from}&to=${to}`);

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
            let parallelArrays = await createParallelArrays(serverData.pings);

            if (selectedMode.id == 0) {
                traces.push(await createSmoothTrace(serverData.serverName, parallelArrays));
            } else if (selectedMode.id == 1) {
                traces.push(await createWeeklyTrace(serverData.serverName, parallelArrays));
            } else {
                traces.push(await createRawTrace(serverData.serverName, parallelArrays));
            }
        }

        let layout = {
            autosize: true // set autosize to rescale
        };    
        let config = {responsive: true};

        loading = false;

        setTimeout(() => {  Plotly.newPlot("plot", traces, layout, config); }, 400);    
    }

    async function createWeeklyTrace(name, parallelArrays) {
        return {
            name: name,
            x: parallelArrays.times,
            y: await smooth(parallelArrays.players, 966),
            mode: 'lines',
            type: 'scatter'
        }
    }

    async function createSmoothTrace(name, parallelArrays) {
        return {
            name: name,
            x: parallelArrays.times,
            y: await smooth(parallelArrays.players, 30),
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

    function movingAvg(array, count, qualifier) {

        // calculate average for subarray
        var avg = function(array, qualifier) {

            var sum = 0, count = 0, val;
            for (var i in array){
                val = array[i];
                if (!qualifier || qualifier(val)) {
                    sum += val;
                    count++;
                }
            }

            return sum / count;
        };

        var result = [], val;

        // pad beginning of result with null values
        for (var i=0; i < count-1; i++)
            result.push(null);

        // calculate average for each subarray and add to result
        for (var i=0, len=array.length - count; i <= len; i++) {

            val = avg(array.slice(i, i + count), qualifier);
            if (isNaN(val))
                result.push(null);
            else
                result.push(val);
        }

        return result;
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