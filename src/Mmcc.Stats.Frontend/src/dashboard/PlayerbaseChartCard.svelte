<script lang="ts">
    import Plot from "./Plot.svelte";
    import Loading from "../Loading.svelte";
    import { fade } from 'svelte/transition';
    import type { IValidationResult } from "../validation";
    import { validateDatePair } from "../validation";
    import type { PingsClient, ServerPlayerbaseChartData } from "../clients";
    import { getDefaultInputDates } from "../dateUtils";
    import type { IDatePeriodStrings } from "../dateUtils";
    
    interface IMode {
        id: number,
        text: string,
    };

    export let pingsClient: PingsClient;

    let modes: IMode[] = [
        { id: 0, text: "Daily rolling average" },
        { id: 1, text: "Weekly rolling average" },
        { id: 2, text: "Raw data" },
    ];
    let showChartDiv: boolean = false;
    let loading: boolean = false;
    let defaultDates: IDatePeriodStrings = getDefaultInputDates();    
    let fromDateInput: string = defaultDates.fromDate;
    let toDateInput: string = defaultDates.toDate;
    let selectedMode: IMode;
    let traces: any[] = [];

    async function handleGoClick(): Promise<void> {
        traces = [];
        showChartDiv = true;
        loading = true;

        let response: ServerPlayerbaseChartData[];
        let validationResult: IValidationResult = validateDatePair(fromDateInput, toDateInput);

        if (validationResult.errorMsg) {
            loading = false;
            showChartDiv = false;
            alert(validationResult.errorMsg);
            return;
        }

        const fromDate: Date = new Date(fromDateInput);
        const toDate: Date = new Date(toDateInput);

        if (selectedMode.id == 0) {
            response = await pingsClient.getChartDataAsAvg(fromDate, toDate, 138);
        } else if (selectedMode.id == 1) {
            response = await pingsClient.getChartDataAsAvg(fromDate, toDate, 966);
        } else if (selectedMode.id == 2) {
            response = await pingsClient.getChartData(fromDate, toDate);
        } else {
            throw new RangeError("selectedMode out of range");
        }

        for (const serverData of response) {
            traces.push(createTrace(serverData));
        }

        loading = false;
    }

    function createTrace(data: ServerPlayerbaseChartData): any {
        return {
            name: data.serverName,
            x: data.times,
            y: data.players,
            mode: 'lines',
            type: 'scatter'
        };
    }
</script>

<div class="card">
    <div class="lg:px-4 lg:my-4">
        <h3 class="mb-4 text-gray-400 tracking-wide uppercase">Playerbase chart</h3>
        <div class="flex flex-wrap gap-3">
            <div>
                <label
                    for="from"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >From</label
                >
                <input
                    bind:value={fromDateInput}
                    id="from"
                    name="from"
                    type="date"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                />
            </div>
            <div>
                <label
                    for="to"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >To</label
                >
                <input
                    bind:value={toDateInput}
                    id="to"
                    name="to"
                    type="date"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                />
            </div>
            <div>
                <label
                    for="mode"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >Mode</label
                >
                <select
                    bind:value={selectedMode}
                    id="mode"
                    name="mode"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                >
                    {#each modes as mode}
                        <option value={mode}>
                            {mode.text}
                        </option>
                    {/each}
                </select>                
                <button on:click={handleGoClick} class="px-8 lg:px-4 py-2 mt-4 sm:mt-0 sm:ml-2 bg-blue-500 text-gray-100 rounded focus:outline-none focus:ring focus:ring-blue-300">Go</button>
            </div>            
        </div>
        <div class="mt-4">
            {#if showChartDiv}
                {#if loading}
                <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent" class="mt-12">
                    <Loading />
                </div>
                {:else}
                    <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent" class="mt-6">
                        <Plot data={traces} />
                    </div>
                {/if}
            {/if}
        </div>
    </div>
</div>
