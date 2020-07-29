<script>
    let chart;
    let from;
    let to;
    let selectedMode;

    let modes = [
        { id: 0, text: "Smoothed data" },
        { id: 1, text: "Raw data" }
    ]

    function handleClick() {
        let trace1 = {
            x: [1, 2, 3, 4],
            y: [10, 15, 13, 17],
            type: 'scatter'
        };

        let trace2 = {
            x: [1, 2, 3, 4],
            y: [16, 5, 11, 9],
            type: 'scatter'
        };

        let data = [trace1, trace2];

        Plotly.newPlot('plot', data);
    }
</script>

<main>
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
        <div id="plot"></div>
    </form>    
</main>

<style>
    main {
        padding: 0;
        display: flex;
        justify-content: center;
    }
</style>