<script lang="ts">
    import Plot from "./Plot.svelte";
    import Loading from "../Loading.svelte";
    import { fade } from 'svelte/transition';
    import type { IValidationResult } from "../validation";
    import { validateDatePair } from "../validation";
    import type { ServerTpsChartData, TpsClient } from "../clients";
    import { getDefaultInputDates } from "../dateUtils";
    import type { IDatePeriodStrings } from "../dateUtils";

    export let tpsClient: TpsClient;

    let showChartDiv: boolean = false;
    let loading: boolean = false;
    let defaultDates: IDatePeriodStrings = getDefaultInputDates();    
    let fromDateInput: string = defaultDates.fromDate;
    let toDateInput: string = defaultDates.toDate;
    let traces: any[] = [];

    async function handleGoClick(): Promise<void> {
        traces = [];
        showChartDiv = true;
        loading = true;

        let validationResult: IValidationResult = validateDatePair(fromDateInput, toDateInput);

        if (validationResult.errorMsg) {
            loading = false;
            showChartDiv = false;
            alert(validationResult.errorMsg);
            return;
        }

        const fromDate: Date = new Date(fromDateInput);
        const toDate: Date = new Date(toDateInput);
        let response: ServerTpsChartData[] = await tpsClient.getChartData(fromDate, toDate);

        for (const serverData of response) {
            traces.push(createTrace(serverData));
        }

        loading = false;
    }

    function createTrace(data: ServerTpsChartData): any {
        return {
            name: data.serverName,
            x: data.times,
            y: data.tps,
            mode: 'lines',
            type: 'scatter',
            line: {
                smoothing: 1.3
            }
        };
    }
</script>

<div class="card">
    <div class="lg:px-4 lg:my-4">
        <h3 class="mb-4 text-gray-400 tracking-wide uppercase">Tps chart</h3>
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
                <button on:click={handleGoClick} class="px-8 lg:px-4 py-2 mt-0 ml-2 bg-blue-500 text-gray-100 rounded focus:outline-none focus:ring focus:ring-blue-300">Go</button>

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